using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Sis_Pdv_Controle_Estoque_API.Exceptions;

namespace Sis_Pdv_Controle_Estoque_API.Services.Validation
{
    /// <summary>
    /// Service for validating models and DTOs
    /// </summary>
    public interface IModelValidationService
    {
        /// <summary>
        /// Validates a model and throws ValidationException if invalid
        /// </summary>
        void ValidateModel<T>(T model) where T : class;

        /// <summary>
        /// Validates a model and returns validation results
        /// </summary>
        ValidationResult ValidateModelWithResults<T>(T model) where T : class;

        /// <summary>
        /// Validates business rules for product creation
        /// </summary>
        Task<ValidationResult> ValidateProductCreationAsync(string codBarras, Guid fornecedorId, Guid categoriaId);

        /// <summary>
        /// Validates business rules for product update
        /// </summary>
        Task<ValidationResult> ValidateProductUpdateAsync(Guid productId, string codBarras, Guid fornecedorId, Guid categoriaId);

        /// <summary>
        /// Validates business rules for stock movement
        /// </summary>
        Task<ValidationResult> ValidateStockMovementAsync(Guid productId, decimal quantity, int type, string? lote, DateTime? dataValidade);
    }

    public class ModelValidationService : IModelValidationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ModelValidationService> _logger;

        public ModelValidationService(IServiceProvider serviceProvider, ILogger<ModelValidationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void ValidateModel<T>(T model) where T : class
        {
            var result = ValidateModelWithResults(model);
            if (!result.IsValid)
            {
                // Log validation errors for metrics
                _logger.LogWarning("Model validation failed for {ModelType}: {Errors}", 
                    typeof(T).Name, string.Join("; ", result.Errors));
                
                throw new Sis_Pdv_Controle_Estoque_API.Exceptions.ValidationException(result.Errors);
            }
        }

        public ValidationResult ValidateModelWithResults<T>(T model) where T : class
        {
            var context = new ValidationContext(model, _serviceProvider, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, results, true);

            var errors = results.Select(r => r.ErrorMessage ?? "Validation error").ToList();

            if (!isValid)
            {
                _logger.LogDebug("Model validation results for {ModelType}: {ErrorCount} errors found", 
                    typeof(T).Name, errors.Count);
            }

            return new ValidationResult
            {
                IsValid = isValid,
                Errors = errors
            };
        }

        public async Task<ValidationResult> ValidateProductCreationAsync(string codBarras, Guid fornecedorId, Guid categoriaId)
        {
            var errors = new List<string>();

            try
            {
                // Check if barcode already exists
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PdvContext>();

                var existingProduct = await context.Produtos
                    .FirstOrDefaultAsync(p => p.CodBarras == codBarras);

                if (existingProduct != null)
                {
                    errors.Add($"Já existe um produto com o código de barras '{codBarras}'");
                }

                // Validate supplier exists
                var supplierExists = await context.Fornecedores
                    .AnyAsync(f => f.Id == fornecedorId);

                if (!supplierExists)
                {
                    errors.Add("Fornecedor não encontrado");
                }

                // Validate category exists
                var categoryExists = await context.Categorias
                    .AnyAsync(c => c.Id == categoriaId);

                if (!categoryExists)
                {
                    errors.Add("Categoria não encontrada");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating product creation");
                errors.Add("Erro interno durante validação");
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        public async Task<ValidationResult> ValidateProductUpdateAsync(Guid productId, string codBarras, Guid fornecedorId, Guid categoriaId)
        {
            var errors = new List<string>();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PdvContext>();

                // Check if product exists
                var product = await context.Produtos.FindAsync(productId);
                if (product == null)
                {
                    errors.Add("Produto não encontrado");
                    return new ValidationResult { IsValid = false, Errors = errors };
                }

                // Check if barcode is used by another product
                var existingProduct = await context.Produtos
                    .FirstOrDefaultAsync(p => p.CodBarras == codBarras && p.Id != productId);

                if (existingProduct != null)
                {
                    errors.Add($"Já existe outro produto com o código de barras '{codBarras}'");
                }

                // Validate supplier exists
                var supplierExists = await context.Fornecedores
                    .AnyAsync(f => f.Id == fornecedorId);

                if (!supplierExists)
                {
                    errors.Add("Fornecedor não encontrado");
                }

                // Validate category exists
                var categoryExists = await context.Categorias
                    .AnyAsync(c => c.Id == categoriaId);

                if (!categoryExists)
                {
                    errors.Add("Categoria não encontrada");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating product update");
                errors.Add("Erro interno durante validação");
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        public async Task<ValidationResult> ValidateStockMovementAsync(Guid productId, decimal quantity, int type, string? lote, DateTime? dataValidade)
        {
            var errors = new List<string>();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PdvContext>();

                // Check if product exists
                var product = await context.Produtos.FindAsync(productId);
                if (product == null)
                {
                    errors.Add("Produto não encontrado");
                    return new ValidationResult { IsValid = false, Errors = errors };
                }

                // Validate perishable product requirements
                if (product.IsPerecivel)
                {
                    if (string.IsNullOrWhiteSpace(lote))
                    {
                        errors.Add("Lote é obrigatório para produtos perecíveis");
                    }

                    if (!dataValidade.HasValue)
                    {
                        errors.Add("Data de validade é obrigatória para produtos perecíveis");
                    }
                    else if (dataValidade.Value <= DateTime.Now)
                    {
                        errors.Add("Data de validade deve ser futura");
                    }
                }

                // Validate stock availability for exits (type 2)
                if (type == 2) // Exit
                {
                    var inventoryService = scope.ServiceProvider.GetRequiredService<Interfaces.Services.IInventoryBalanceService>();
                    var currentBalance = await inventoryService.CalculateCurrentBalanceAsync(productId, CancellationToken.None);
                    
                    if (currentBalance != null && currentBalance.AvailableStock < quantity)
                    {
                        errors.Add($"Estoque insuficiente. Disponível: {currentBalance.AvailableStock}, Solicitado: {quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stock movement");
                errors.Add("Erro interno durante validação");
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
