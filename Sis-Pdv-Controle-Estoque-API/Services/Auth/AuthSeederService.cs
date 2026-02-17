using Repositories.Base;
using Interfaces;
using Model;
using Microsoft.EntityFrameworkCore;

namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public class AuthSeederService
    {
        private readonly IRepositoryRole _roleRepository;
        private readonly IRepositoryPermission _permissionRepository;
        private readonly IRepositoryUserRole _userRoleRepository;
        private readonly IRepositoryRolePermission _rolePermissionRepository;
        private readonly IRepositoryUsuario _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<AuthSeederService> _logger;
        private readonly PdvContext _context;

        public AuthSeederService(
            IRepositoryRole roleRepository,
            IRepositoryPermission permissionRepository,
            IRepositoryUserRole userRoleRepository,
            IRepositoryRolePermission rolePermissionRepository,
            IRepositoryUsuario userRepository,
            IPasswordService passwordService,
            ILogger<AuthSeederService> logger,
            PdvContext context)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
            _context = context;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedPermissionsAsync();
                await SeedRolesAsync();
                
                // Save permissions and roles to DB before referencing them in subsequent seeds
                await _context.SaveChangesAsync();
                
                await SeedRolePermissionsAsync();
                await SeedDefaultUsersAsync();
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Authentication data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding authentication data");
                throw;
            }
        }

        // Ensure admin exists and has Administrator role, safe to call every startup
        public async Task EnsureAdminUserAndRolesAsync()
        {
            try
            {
                // Ensure Administrator role exists
                var adminRole = await _roleRepository.GetByNameAsync("Administrator");
                if (adminRole == null)
                {
                    adminRole = await _roleRepository.AdicionarAsync(new Role
                    {
                        Name = "Administrator",
                        Description = "Administrador do sistema com acesso total"
                    });
                }

                // Ensure HeitorAdmin user exists
                var adminUser = await _userRepository.GetByLoginAsync("HeitorAdmin");
                if (adminUser == null)
                {
                    adminUser = await _userRepository.AdicionarAsync(new Usuario
                    {
                        Login = "HeitorAdmin",
                        Email = "heitoradmin@pdv.com",
                        Nome = "Heitor Admin",
                        Senha = _passwordService.HashPassword("HS1384@"),
                        StatusAtivo = true
                    });
                }

                // Ensure mapping exists and is not soft-deleted
                var existingMapping = await _context.UserRoles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);
                
                if (existingMapping == null)
                {
                    await _userRoleRepository.AdicionarAsync(new UserRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id
                    });
                }
                else if (existingMapping.IsDeleted)
                {
                    // Reactivate soft-deleted mapping
                    existingMapping.IsDeleted = false;
                    existingMapping.DeletedAt = null;
                    existingMapping.DeletedBy = null;
                    existingMapping.UpdatedAt = DateTime.UtcNow;
                }

                // Also ensure caixa1 and fiscal1 have their role mappings
                await EnsureUserRoleMappingAsync("caixa1", "Cashier");
                await EnsureUserRoleMappingAsync("fiscal1", "CashSupervisor");

                // Ensure role permissions exist (fix for initial seed ordering bug)
                await EnsureRolePermissionsAsync();

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ensuring admin user and roles");
                throw;
            }
        }

        /// <summary>
        /// Ensures a user has a specific role mapping, creating or reactivating as needed
        /// </summary>
        private async Task EnsureUserRoleMappingAsync(string login, string roleName)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null) return;

            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null) return;

            var existing = await _context.UserRoles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

            if (existing == null)
            {
                await _userRoleRepository.AdicionarAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }
            else if (existing.IsDeleted)
            {
                existing.IsDeleted = false;
                existing.DeletedAt = null;
                existing.DeletedBy = null;
                existing.UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Ensures role permissions exist. Idempotent — only creates missing mappings.
        /// Fixes databases where the initial seed ran before roles were committed.
        /// </summary>
        private async Task EnsureRolePermissionsAsync()
        {
            var hasAnyRolePermissions = await _context.RolePermissions.AnyAsync();
            if (hasAnyRolePermissions) return; // Already seeded, nothing to do

            _logger.LogInformation("No role permissions found — seeding them now");
            await SeedRolePermissionsAsync();
        }

        private async Task SeedPermissionsAsync()
        {
            var permissions = new[]
            {
                // User Management
                new Permission { Name = "users.view", Description = "Visualizar usuários", Resource = "users", Action = "view" },
                new Permission { Name = "users.create", Description = "Criar usuários", Resource = "users", Action = "create" },
                new Permission { Name = "users.edit", Description = "Editar usuários", Resource = "users", Action = "edit" },
                new Permission { Name = "users.delete", Description = "Excluir usuários", Resource = "users", Action = "delete" },
                
                // Product Management
                new Permission { Name = "products.view", Description = "Visualizar produtos", Resource = "products", Action = "view" },
                new Permission { Name = "products.create", Description = "Criar produtos", Resource = "products", Action = "create" },
                new Permission { Name = "products.edit", Description = "Editar produtos", Resource = "products", Action = "edit" },
                new Permission { Name = "products.delete", Description = "Excluir produtos", Resource = "products", Action = "delete" },
                
                // Customer Management
                new Permission { Name = "customers.view", Description = "Visualizar clientes", Resource = "customers", Action = "view" },
                new Permission { Name = "customers.create", Description = "Criar clientes", Resource = "customers", Action = "create" },
                new Permission { Name = "customers.edit", Description = "Editar clientes", Resource = "customers", Action = "edit" },
                new Permission { Name = "customers.delete", Description = "Excluir clientes", Resource = "customers", Action = "delete" },
                
                // Sales Management
                new Permission { Name = "sales.view", Description = "Visualizar vendas", Resource = "sales", Action = "view" },
                new Permission { Name = "sales.create", Description = "Realizar vendas", Resource = "sales", Action = "create" },
                new Permission { Name = "sales.edit", Description = "Editar vendas", Resource = "sales", Action = "edit" },
                new Permission { Name = "sales.cancel", Description = "Cancelar vendas", Resource = "sales", Action = "cancel" },
                
                // Inventory Management
                new Permission { Name = "inventory.view", Description = "Visualizar estoque", Resource = "inventory", Action = "view" },
                new Permission { Name = "inventory.adjust", Description = "Ajustar estoque", Resource = "inventory", Action = "adjust" },
                
                // Reports
                new Permission { Name = "reports.view", Description = "Visualizar relatórios", Resource = "reports", Action = "view" },
                new Permission { Name = "reports.export", Description = "Exportar relatórios", Resource = "reports", Action = "export" },
                
                // System Administration
                new Permission { Name = "system.admin", Description = "Administração do sistema", Resource = "system", Action = "admin" },
                new Permission { Name = "system.backup", Description = "Realizar backup", Resource = "system", Action = "backup" },
                new Permission { Name = "system.restore", Description = "Restaurar backup", Resource = "system", Action = "restore" }
            };

            foreach (var permission in permissions)
            {
                var existingPermission = await _permissionRepository.GetByNameAsync(permission.Name);
                if (existingPermission == null)
                {
                    await _permissionRepository.AdicionarAsync(permission);
                }
            }
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[]
            {
                new Role { Name = "Administrator", Description = "Administrador do sistema com acesso total" },
                new Role { Name = "Manager", Description = "Gerente com acesso a relatórios e configurações" },
                new Role { Name = "Cashier", Description = "Operador de caixa com acesso a vendas" },
                new Role { Name = "StockManager", Description = "Gerente de estoque com acesso ao inventário" },
                new Role { Name = "CashSupervisor", Description = "Fiscal de caixa com poderes de supervisão" }
            };

            foreach (var role in roles)
            {
                var existingRole = await _roleRepository.GetByNameAsync(role.Name);
                if (existingRole == null)
                {
                    await _roleRepository.AdicionarAsync(role);
                }
            }
        }

        private async Task SeedRolePermissionsAsync()
        {
            // Administrator - all permissions
            var adminRole = await _roleRepository.GetByNameAsync("Administrator");
            if (adminRole != null)
            {
                var allPermissions = _permissionRepository.Listar().ToList();
                foreach (var permission in allPermissions)
                {
                    var existingRolePermission = _context.RolePermissions
                        .FirstOrDefault(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id);
                    
                    if (existingRolePermission == null)
                    {
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            RoleId = adminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            // Manager - management permissions
            var managerRole = await _roleRepository.GetByNameAsync("Manager");
            if (managerRole != null)
            {
                var managerPermissions = new[]
                {
                    "users.view", "products.view", "products.create", "products.edit",
                    "customers.view", "customers.create", "customers.edit",
                    "sales.view", "sales.create", "inventory.view", "inventory.adjust",
                    "reports.view", "reports.export"
                };

                foreach (var permissionName in managerPermissions)
                {
                    var permission = await _permissionRepository.GetByNameAsync(permissionName);
                    if (permission != null)
                    {
                        var existingRolePermission = _context.RolePermissions
                            .FirstOrDefault(rp => rp.RoleId == managerRole.Id && rp.PermissionId == permission.Id);
                        
                        if (existingRolePermission == null)
                        {
                            await _context.RolePermissions.AddAsync(new RolePermission
                            {
                                RoleId = managerRole.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }
            }

            // Cashier - sales permissions
            var cashierRole = await _roleRepository.GetByNameAsync("Cashier");
            if (cashierRole != null)
            {
                var cashierPermissions = new[]
                {
                    "products.view", "customers.view", "customers.create",
                    "sales.view", "sales.create", "inventory.view"
                };

                foreach (var permissionName in cashierPermissions)
                {
                    var permission = await _permissionRepository.GetByNameAsync(permissionName);
                    if (permission != null)
                    {
                        var existingRolePermission = _context.RolePermissions
                            .FirstOrDefault(rp => rp.RoleId == cashierRole.Id && rp.PermissionId == permission.Id);
                        
                        if (existingRolePermission == null)
                        {
                            await _context.RolePermissions.AddAsync(new RolePermission
                            {
                                RoleId = cashierRole.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }
            }

            // StockManager - inventory permissions
            var stockManagerRole = await _roleRepository.GetByNameAsync("StockManager");
            if (stockManagerRole != null)
            {
                var stockManagerPermissions = new[]
                {
                    "products.view", "products.create", "products.edit",
                    "inventory.view", "inventory.adjust", "reports.view"
                };

                foreach (var permissionName in stockManagerPermissions)
                {
                    var permission = await _permissionRepository.GetByNameAsync(permissionName);
                    if (permission != null)
                    {
                        var existingRolePermission = _context.RolePermissions
                            .FirstOrDefault(rp => rp.RoleId == stockManagerRole.Id && rp.PermissionId == permission.Id);
                        
                        if (existingRolePermission == null)
                        {
                            await _context.RolePermissions.AddAsync(new RolePermission
                            {
                                RoleId = stockManagerRole.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }
            }

            // CashSupervisor - cashier supervisor permissions
            var cashSupervisorRole = await _roleRepository.GetByNameAsync("CashSupervisor");
            if (cashSupervisorRole != null)
            {
                var supervisorPermissions = new[]
                {
                    "products.view", "customers.view", "customers.create",
                    "sales.view", "sales.create", "sales.cancel", "inventory.view", "reports.view"
                };

                foreach (var permissionName in supervisorPermissions)
                {
                    var permission = await _permissionRepository.GetByNameAsync(permissionName);
                    if (permission != null)
                    {
                        var existingRolePermission = _context.RolePermissions
                            .FirstOrDefault(rp => rp.RoleId == cashSupervisorRole.Id && rp.PermissionId == permission.Id);
                        
                        if (existingRolePermission == null)
                        {
                            await _context.RolePermissions.AddAsync(new RolePermission
                            {
                                RoleId = cashSupervisorRole.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }
            }
        }

        private async Task SeedDefaultUsersAsync()
        {
            // Admin user (as requested)
            var existingAdmin = await _userRepository.GetByLoginAsync("HeitorAdmin");
            if (existingAdmin == null)
            {
                var adminUser = new Usuario
                {
                    Login = "HeitorAdmin",
                    Email = "heitoradmin@pdv.com",
                    Nome = "Heitor Admin",
                    Senha = _passwordService.HashPassword("HS1384@"),
                    StatusAtivo = true
                };

                await _userRepository.AdicionarAsync(adminUser);

                var adminRole = await _roleRepository.GetByNameAsync("Administrator");
                if (adminRole != null)
                {
                    await _userRoleRepository.AdicionarAsync(new UserRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id
                    });
                }
            }

            // Default cashier
            var cashierUser = await _userRepository.GetByLoginAsync("caixa1");
            if (cashierUser == null)
            {
                cashierUser = new Usuario
                {
                    Login = "caixa1",
                    Email = "caixa1@pdv.com",
                    Nome = "Caixa Principal",
                    Senha = _passwordService.HashPassword("Caixa@123"),
                    StatusAtivo = true
                };
                await _userRepository.AdicionarAsync(cashierUser);

                var cashierRole = await _roleRepository.GetByNameAsync("Cashier");
                if (cashierRole != null)
                {
                    await _userRoleRepository.AdicionarAsync(new UserRole
                    {
                        UserId = cashierUser.Id,
                        RoleId = cashierRole.Id
                    });
                }
            }

            // Default cash supervisor (fiscal de caixa)
            var supervisorUser = await _userRepository.GetByLoginAsync("fiscal1");
            if (supervisorUser == null)
            {
                supervisorUser = new Usuario
                {
                    Login = "fiscal1",
                    Email = "fiscal1@pdv.com",
                    Nome = "Fiscal de Caixa",
                    Senha = _passwordService.HashPassword("Fiscal@123"),
                    StatusAtivo = true
                };
                await _userRepository.AdicionarAsync(supervisorUser);

                var cashSupervisorRole = await _roleRepository.GetByNameAsync("CashSupervisor");
                if (cashSupervisorRole != null)
                {
                    await _userRoleRepository.AdicionarAsync(new UserRole
                    {
                        UserId = supervisorUser.Id,
                        RoleId = cashSupervisorRole.Id
                    });
                }
            }
        }
    }
}