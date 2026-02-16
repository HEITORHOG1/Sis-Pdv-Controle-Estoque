using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Repositories.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public AuditInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var currentUserId = GetCurrentUserId();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.CreatedBy = currentUserId;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        // Don't update CreatedAt and CreatedBy for modified entities
                        entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                        entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
                        
                        entry.Entity.UpdatedAt = utcNow;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;

                    case EntityState.Deleted:
                        // Implement soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = utcNow;
                        entry.Entity.DeletedBy = currentUserId;
                        break;
                }
            }

            // Create audit logs for tracked changes
            CreateAuditLogs(context, currentUserId, utcNow);
        }

        private void CreateAuditLogs(DbContext context, Guid? currentUserId, DateTime timestamp)
        {
            if (currentUserId == null) return;

            var auditLogs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // Skip audit logs themselves to avoid infinite loops
                if (entry.Entity is AuditLog) continue;

                // Only track entities that inherit from EntityBase
                if (entry.Entity is not EntityBase) continue;

                var entityId = ((EntityBase)entry.Entity).Id;
                var entityName = entry.Entity.GetType().Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditLogs.Add(new AuditLog
                        {
                            EntityName = entityName,
                            EntityId = entityId,
                            Action = "INSERT",
                            UserId = currentUserId.Value,
                            Timestamp = timestamp,
                            NewValues = SerializeEntity(entry.CurrentValues),
                            Changes = "Entity created"
                        });
                        break;

                    case EntityState.Modified:
                        var originalValues = SerializeEntity(entry.OriginalValues);
                        var currentValues = SerializeEntity(entry.CurrentValues);
                        var changes = GetChangedProperties(entry);

                        if (changes.Any())
                        {
                            auditLogs.Add(new AuditLog
                            {
                                EntityName = entityName,
                                EntityId = entityId,
                                Action = entry.Entity is IAuditableEntity auditable && auditable.IsDeleted ? "DELETE" : "UPDATE",
                                UserId = currentUserId.Value,
                                Timestamp = timestamp,
                                OldValues = originalValues,
                                NewValues = currentValues,
                                Changes = string.Join(", ", changes)
                            });
                        }
                        break;
                }
            }

            // Add audit logs to context
            if (auditLogs.Any())
            {
                context.Set<AuditLog>().AddRange(auditLogs);
            }
        }

        private string SerializeEntity(Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues values)
        {
            var dictionary = new Dictionary<string, object?>();
            foreach (var property in values.Properties)
            {
                dictionary[property.Name] = values[property];
            }
            return JsonSerializer.Serialize(dictionary, new JsonSerializerOptions { WriteIndented = false });
        }

        private List<string> GetChangedProperties(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            var changes = new List<string>();
            
            foreach (var property in entry.Properties)
            {
                if (property.IsModified && !IsAuditProperty(property.Metadata.Name))
                {
                    var originalValue = property.OriginalValue?.ToString() ?? "null";
                    var currentValue = property.CurrentValue?.ToString() ?? "null";
                    changes.Add($"{property.Metadata.Name}: '{originalValue}' -> '{currentValue}'");
                }
            }

            return changes;
        }

        private bool IsAuditProperty(string propertyName)
        {
            return propertyName is nameof(IAuditableEntity.CreatedAt) or 
                                  nameof(IAuditableEntity.CreatedBy) or 
                                  nameof(IAuditableEntity.UpdatedAt) or 
                                  nameof(IAuditableEntity.UpdatedBy) or 
                                  nameof(IAuditableEntity.IsDeleted) or 
                                  nameof(IAuditableEntity.DeletedAt) or 
                                  nameof(IAuditableEntity.DeletedBy);
        }

        private Guid? GetCurrentUserId()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
                
                if (httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                    ?? httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value
                                    ?? httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
                    
                    if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
                    {
                        return userId;
                    }
                }
                
                return null;
            }
            catch
            {
                // If we can't get the current user ID, return null
                // This can happen during migrations or background tasks
                return null;
            }
        }
    }
}