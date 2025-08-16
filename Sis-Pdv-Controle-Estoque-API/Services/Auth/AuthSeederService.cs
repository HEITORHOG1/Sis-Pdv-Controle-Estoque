using Repositories.Base;
using Interfaces;
using Model;

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
                await SeedRolePermissionsAsync();
                await SeedDefaultUserAsync();
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Authentication data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding authentication data");
                throw;
            }
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
                new Role { Name = "StockManager", Description = "Gerente de estoque com acesso ao inventário" }
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
        }

        private async Task SeedDefaultUserAsync()
        {
            var existingUser = await _userRepository.GetByLoginAsync("admin");
            if (existingUser == null)
            {
                var adminUser = new Usuario
                {
                    Login = "admin",
                    Email = "admin@pdv.com",
                    Nome = "Administrador",
                    Senha = _passwordService.HashPassword("admin123"),
                    StatusAtivo = true
                };

                await _userRepository.AdicionarAsync(adminUser);

                // Assign Administrator role
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
        }
    }
}