using Repositories.Base;
using Model;
using Microsoft.EntityFrameworkCore;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Services
{
    /// <summary>
    /// Seeds domain data across all tables with at least 5 records each, idempotently.
    /// </summary>
    public class DomainSeederService
    {
        private readonly PdvContext _context;
        private readonly ILogger<DomainSeederService> _logger;
        private readonly IPasswordService _passwordService;

        public DomainSeederService(PdvContext context, ILogger<DomainSeederService> logger, IPasswordService passwordService)
        {
            _context = context;
            _logger = logger;
            _passwordService = passwordService;
        }

        public async Task SeedAsync()
        {
            try
            {
                // Base master data
                await SeedDepartamentosAsync(5);
                await SeedCategoriasAsync(5);
                await SeedFornecedoresAsync(5);
                await SeedClientesAsync(5);

                // Users and roles relations (Auth tables are seeded in AuthSeederService, we complement here)
                await SeedExtraRolesIfNeededAsync(5);
                await SeedUsersAsync(5);
                await SeedUserRolesAsync(5);

                // Products and stock
                await SeedProdutosAsync(5);
                await SeedStockMovementsAsync(5);

                // Colaboradores
                await SeedColaboradoresAsync(5);

                // Orders and related financial entities
                await SeedPedidosCompletoAsync(5);

                // Sessions
                await SeedUserSessionsAsync(5);

                // Audit logs
                await SeedAuditLogsAsync(5);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Domain data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding domain data");
                throw;
            }
        }

        private async Task SeedDepartamentosAsync(int min)
        {
            var names = new[] { "Vendas", "Estoque", "Financeiro", "Administra��o", "Atendimento" };
            foreach (var n in names)
            {
                if (!await _context.Departamentos.AnyAsync(x => x.NomeDepartamento == n))
                {
                    await _context.Departamentos.AddAsync(new Departamento { NomeDepartamento = n });
                }
            }
        }

        private async Task SeedCategoriasAsync(int min)
        {
            var names = new[] { "Bebidas", "Lanches", "Limpeza", "Higiene", "Padaria" };
            foreach (var n in names)
            {
                if (!await _context.Categorias.AnyAsync(x => x.NomeCategoria == n))
                {
                    await _context.Categorias.AddAsync(new Categoria { NomeCategoria = n });
                }
            }
        }

        private async Task SeedFornecedoresAsync(int min)
        {
            var baseList = Enumerable.Range(1, min).Select(i => new Fornecedor
            {
                InscricaoEstadual = $"IE{i:000000}",
                NomeFantasia = $"Fornecedor {i}",
                Uf = "SP",
                Numero = (100 + i).ToString(),
                Complemento = $"Sala {i}",
                Bairro = "Centro",
                Cidade = "S�o Paulo",
                CepFornecedor = 01000000 + i,
                StatusAtivo = 1,
                Cnpj = $"1234567800019{i}",
                Rua = $"Av. Principal {i}"
            }).ToList();

            foreach (var f in baseList)
            {
                if (!await _context.Fornecedores.AnyAsync(x => x.Cnpj == f.Cnpj))
                {
                    await _context.Fornecedores.AddAsync(f);
                }
            }
        }

        private async Task SeedClientesAsync(int min)
        {
            var list = Enumerable.Range(1, min).Select(i => new Cliente
            {
                CpfCnpj = $"0000000000{i}",
                TipoCliente = i % 2 == 0 ? "PessoaFisica" : "PessoaJuridica"
            });

            foreach (var c in list)
            {
                if (!await _context.Clientes.AnyAsync(x => x.CpfCnpj == c.CpfCnpj))
                {
                    await _context.Clientes.AddAsync(c);
                }
            }
        }

        private async Task SeedExtraRolesIfNeededAsync(int min)
        {
            var existing = await _context.Roles.CountAsync();
            var toAdd = new[]
            {
                new Role { Name = "Support", Description = "Suporte ao usu�rio" },
                new Role { Name = "Auditor", Description = "Auditoria e conformidade" },
                new Role { Name = "Operator", Description = "Operador geral da loja" }
            };
            foreach (var r in toAdd)
            {
                if (await _context.Roles.CountAsync() >= min) break;
                if (!await _context.Roles.AnyAsync(x => x.Name == r.Name))
                {
                    await _context.Roles.AddAsync(r);
                }
            }
        }

        private async Task SeedUsersAsync(int min)
        {
            // Admin is created by AuthSeederService. Create more users to reach 'min'.
            var templates = new[]
            {
                new { Login = "user1", Email = "user1@pdv.com", Nome = "Usu�rio 1" },
                new { Login = "user2", Email = "user2@pdv.com", Nome = "Usu�rio 2" },
                new { Login = "user3", Email = "user3@pdv.com", Nome = "Usu�rio 3" },
                new { Login = "user4", Email = "user4@pdv.com", Nome = "Usu�rio 4" },
                new { Login = "user5", Email = "user5@pdv.com", Nome = "Usu�rio 5" }
            };

            foreach (var t in templates)
            {
                if (!await _context.Usuarios.AnyAsync(u => u.Login == t.Login))
                {
                    await _context.Usuarios.AddAsync(new Usuario
                    {
                        Login = t.Login,
                        Email = t.Email,
                        Nome = t.Nome,
                        Senha = _passwordService.HashPassword("Password123!"),
                        StatusAtivo = true
                    });
                }
            }
        }

        private async Task SeedUserRolesAsync(int min)
        {
            var users = await _context.Usuarios.ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            if (!users.Any() || !roles.Any()) return;

            // assign each user a role (round-robin) to ensure at least 'min' mappings
            foreach (var (user, index) in users.Select((u, idx) => (u, idx)))
            {
                var role = roles[index % roles.Count];
                if (!await _context.UserRoles.IgnoreQueryFilters().AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                {
                    await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
                }
            }

            // ensure at least 'min' total
            if (await _context.UserRoles.CountAsync() < min && users.Count > 1 && roles.Count > 1)
            {
                foreach (var user in users)
                {
                    foreach (var role in roles)
                    {
                        if (await _context.UserRoles.CountAsync() >= min) break;
                        if (!await _context.UserRoles.IgnoreQueryFilters().AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                        {
                            await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
                        }
                    }
                    if (await _context.UserRoles.CountAsync() >= min) break;
                }
            }
        }

        private async Task SeedProdutosAsync(int min)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync();
            var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync();
            if (categoria == null || fornecedor == null) return;

            var list = Enumerable.Range(1, min).Select(i => new Produto
            {
                CodBarras = $"78900000010{i}",
                NomeProduto = $"Produto {i}",
                DescricaoProduto = $"Descri��o do produto {i}",
                IsPerecivel = i % 3 == 0, // Alguns produtos perecíveis





                FornecedorId = fornecedor.Id,
                CategoriaId = categoria.Id,
                StatusAtivo = 1
            });

            foreach (var p in list)
            {
                if (!await _context.Produtos.AnyAsync(x => x.CodBarras == p.CodBarras))
                {
                    await _context.Produtos.AddAsync(p);
                }
            }
        }

        private async Task SeedStockMovementsAsync(int min)
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (!produtos.Any()) return;

            int added = 0;
            foreach (var prod in produtos)
            {
                if (added >= min) break;
                if (!await _context.StockMovements.AnyAsync(sm => sm.ProdutoId == prod.Id))
                {
                    await _context.StockMovements.AddAsync(new StockMovement
                    {
                        ProdutoId = prod.Id,
                        Quantity = 10,
                        Type = StockMovementType.Entry,
                        Reason = "Estoque inicial",
                        UnitCost = 2.50m,
                        PreviousStock = 0,
                        NewStock = 10,
                        MovementDate = DateTime.UtcNow
                    });
                    added++;
                }
            }
        }

        private async Task SeedColaboradoresAsync(int min)
        {
            var users = await _context.Usuarios.ToListAsync();
            var departamentos = await _context.Departamentos.ToListAsync();
            if (!users.Any() || !departamentos.Any()) return;

            var existCount = await _context.Colaboradores.CountAsync();
            var need = Math.Max(0, min - existCount);
            int i = 0;
            foreach (var user in users)
            {
                if (need <= 0) break;
                if (!await _context.Colaboradores.AnyAsync(c => c.Usuario.Id == user.Id))
                {
                    var dep = departamentos[i % departamentos.Count];
                    i++;
                    await _context.Colaboradores.AddAsync(new Colaborador
                    {
                        NomeColaborador = user.Nome ?? user.Login,
                        DepartamentoId = dep.Id,
                        CpfColaborador = $"111111111{(i % 9)}",
                        CargoColaborador = "Operador",
                        TelefoneColaborador = "11999999999",
                        EmailPessoalColaborador = $"{user.Login}@mail.com",
                        EmailCorporativo = $"{user.Login}@pdv.com",
                        Usuario = user
                    });
                    need--;
                }
            }
        }

        private async Task SeedPedidosCompletoAsync(int min)
        {
            var clientes = await _context.Clientes.ToListAsync();
            var colaboradores = await _context.Colaboradores.ToListAsync();
            var produtos = await _context.Produtos.ToListAsync();
            if (!clientes.Any() || !colaboradores.Any() || !produtos.Any()) return;

            var exist = await _context.Pedidos.CountAsync();
            int need = Math.Max(0, min - exist);
            int orderIndex = 0;

            while (need-- > 0)
            {
                var cli = clientes[orderIndex % clientes.Count];
                var col = colaboradores[orderIndex % colaboradores.Count];
                orderIndex++;
                var pedido = new Pedido
                {
                    Status = 1,
                    DataDoPedido = DateTime.UtcNow.AddMinutes(-orderIndex * 10),
                    FormaPagamento = orderIndex % 2 == 0 ? "Cart�o de Cr�dito" : "Dinheiro",
                    TotalPedido = 0m,
                    ColaboradorId = col.Id,
                    ClienteId = cli.Id
                };
                await _context.Pedidos.AddAsync(pedido);

                // Items
                var itemsCount = 2 + (orderIndex % 3); // 2-4 items
                decimal total = 0m;
                for (int k = 0; k < itemsCount; k++)
                {
                    var prod = produtos[(orderIndex + k) % produtos.Count];
                    var qty = 1 + (k % 3);
                    var itemTotal = 4.00m * qty; // Preço fixo para seed
                    total += itemTotal;
                    await _context.ProdutoPedidos.AddAsync(new ProdutoPedido
                    {
                        Pedido = pedido,
                        Produto = prod,
                        CodBarras = prod.CodBarras,
                        QuantidadeItemPedido = qty,
                        TotalProdutoPedido = itemTotal
                    });
                }
                pedido.TotalPedido = total;

                // Cupom
                await _context.Cupoms.AddAsync(new Cupom
                {
                    Pedido = pedido,
                    DataEmissao = DateTime.UtcNow,
                    NumeroSerie = $"NS-{Guid.NewGuid().ToString("N").Substring(0,8)}",
                    ChaveAcesso = $"CH-{Guid.NewGuid().ToString("N").Substring(0,12)}",
                    ValorTotal = total,
                    DocumentoCliente = cli.CpfCnpj
                });

                // Payment
                var payment = new Model.Payment
                {
                    Order = pedido,
                    TotalAmount = total,
                    Status = PaymentStatus.Processed,
                    PaymentDate = DateTime.UtcNow
                };
                await _context.Payments.AddAsync(payment);

                // PaymentItems
                await _context.PaymentItems.AddAsync(new PaymentItem
                {
                    Payment = payment,
                    Method = orderIndex % 2 == 0 ? PaymentMethod.CreditCard : PaymentMethod.Cash,
                    Amount = total,
                    Status = PaymentItemStatus.Approved,
                    ProcessedAt = DateTime.UtcNow
                });

                // PaymentAudit
                var anyUser = await _context.Usuarios.FirstOrDefaultAsync();
                if (anyUser != null)
                {
                    await _context.PaymentAudits.AddAsync(new PaymentAudit
                    {
                        Payment = payment,
                        Action = PaymentAuditAction.Processed,
                        Description = "Pagamento processado",
                        User = anyUser,
                        ActionDate = DateTime.UtcNow
                    });
                }

                // FiscalReceipt
                await _context.FiscalReceipts.AddAsync(new FiscalReceipt
                {
                    Payment = payment,
                    ReceiptNumber = $"RC-{Guid.NewGuid().ToString("N").Substring(0,6)}",
                    SerialNumber = $"SN-{Guid.NewGuid().ToString("N").Substring(0,6)}",
                    IssuedAt = DateTime.UtcNow,
                    Status = FiscalReceiptStatus.Authorized,
                    AccessKey = $"AK{Guid.NewGuid().ToString("N").Substring(0,10)}"
                });
            }
        }

        private async Task SeedUserSessionsAsync(int min)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync();
            if (user == null) return;
            var exist = await _context.UserSessions.CountAsync();
            for (int i = exist; i < min; i++)
            {
                await _context.UserSessions.AddAsync(new UserSession
                {
                    User = user,
                    SessionToken = Guid.NewGuid().ToString("N"),
                    IpAddress = "127.0.0.1",
                    UserAgent = "Seeder/1.0",
                    LoginAt = DateTime.UtcNow.AddMinutes(-i * 5),
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    IsActive = true
                });
            }
        }

        private async Task SeedAuditLogsAsync(int min)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync();
            if (user == null) return;
            var exist = await _context.AuditLogs.CountAsync();
            for (int i = exist; i < min; i++)
            {
                await _context.AuditLogs.AddAsync(new AuditLog
                {
                    EntityName = "SeedOperation",
                    EntityId = Guid.NewGuid(),
                    Action = "INSERT",
                    Changes = $"{{\"message\":\"seed {i}\"}}",
                    User = user,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
