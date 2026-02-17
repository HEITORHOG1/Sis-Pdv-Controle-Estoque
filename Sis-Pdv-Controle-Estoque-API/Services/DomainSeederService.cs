using Repositories.Base;
using Model;
using Microsoft.EntityFrameworkCore;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Services
{
    /// <summary>
    /// Seeds realistic demo data for all domain tables.
    /// Uses real Brazilian product names, valid CPFs/CNPJs, and realistic prices.
    /// Idempotent — safe to run multiple times.
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
                _logger.LogInformation("Iniciando seed de dados de demonstracao");

                // 1. Dados mestres (sem dependencias)
                await SeedDepartamentosAsync();
                await SeedCategoriasAsync();
                await SeedFornecedoresAsync();
                await SeedClientesAsync();

                // 2. Usuarios complementares (AuthSeeder ja cria HeitorAdmin, caixa1, fiscal1)
                await SeedUsersAsync();
                await SeedUserRolesAsync();

                // 3. Produtos (dependem de Categoria + Fornecedor)
                await SeedProdutosAsync();
                await SeedStockMovementsAsync();

                // 4. Colaboradores (dependem de Usuario + Departamento)
                await SeedColaboradoresAsync();

                // 5. Pedidos completos (Pedido + ProdutoPedido + Cupom + Payment + FiscalReceipt)
                await SeedPedidosCompletoAsync();

                // 6. Sessoes e Auditoria
                await SeedUserSessionsAsync();
                await SeedAuditLogsAsync();

                await _context.SaveChangesAsync();
                _logger.LogInformation("Seed de dados de demonstracao concluido com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar seed de dados de demonstracao");
                throw;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // DEPARTAMENTOS
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedDepartamentosAsync()
        {
            var departamentos = new[]
            {
                "Vendas", "Estoque", "Financeiro", "Recursos Humanos",
                "Atendimento ao Cliente", "Compras", "TI"
            };

            foreach (var nome in departamentos)
            {
                if (!await _context.Departamentos.AnyAsync(x => x.NomeDepartamento == nome))
                {
                    await _context.Departamentos.AddAsync(new Departamento { NomeDepartamento = nome });
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // CATEGORIAS (supermercado real)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedCategoriasAsync()
        {
            var categorias = new[]
            {
                "Bebidas", "Laticinios", "Padaria e Confeitaria",
                "Frios e Embutidos", "Mercearia", "Limpeza",
                "Higiene Pessoal", "Hortifruti", "Carnes e Aves",
                "Doces e Snacks", "Congelados", "Cereais e Graos"
            };

            foreach (var nome in categorias)
            {
                if (!await _context.Categorias.AnyAsync(x => x.NomeCategoria == nome))
                {
                    await _context.Categorias.AddAsync(new Categoria { NomeCategoria = nome });
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // FORNECEDORES (empresas reais brasileiras)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedFornecedoresAsync()
        {
            var fornecedores = new[]
            {
                new Fornecedor
                {
                    InscricaoEstadual = "IE110482106",
                    NomeFantasia = "Ambev S/A",
                    Uf = "SP",
                    Numero = "1000",
                    Complemento = "Galpao 1",
                    Bairro = "Itaim Bibi",
                    Cidade = "Sao Paulo",
                    CepFornecedor = 04538132,
                    StatusAtivo = 1,
                    Cnpj = "07526557000100",
                    Rua = "Rua Dr. Renato Paes de Barros"
                },
                new Fornecedor
                {
                    InscricaoEstadual = "IE206345891",
                    NomeFantasia = "Nestle Brasil Ltda",
                    Uf = "SP",
                    Numero = "900",
                    Complemento = "Bloco A",
                    Bairro = "Vila Mariana",
                    Cidade = "Sao Paulo",
                    CepFornecedor = 04101000,
                    StatusAtivo = 1,
                    Cnpj = "60409075000152",
                    Rua = "Av. Dr. Chucri Zaidan"
                },
                new Fornecedor
                {
                    InscricaoEstadual = "IE304567123",
                    NomeFantasia = "BRF S/A (Sadia / Perdigao)",
                    Uf = "SC",
                    Numero = "500",
                    Complemento = "CD Principal",
                    Bairro = "Concordia",
                    Cidade = "Concordia",
                    CepFornecedor = 89700000,
                    StatusAtivo = 1,
                    Cnpj = "01838723000127",
                    Rua = "Rua Jorge Tzachel"
                },
                new Fornecedor
                {
                    InscricaoEstadual = "IE412345678",
                    NomeFantasia = "Unilever Brasil Ltda",
                    Uf = "SP",
                    Numero = "3532",
                    Complemento = "Torre Sul",
                    Bairro = "Pinheiros",
                    Cidade = "Sao Paulo",
                    CepFornecedor = 05425070,
                    StatusAtivo = 1,
                    Cnpj = "01615814000172",
                    Rua = "Av. Brigadeiro Faria Lima"
                },
                new Fornecedor
                {
                    InscricaoEstadual = "IE523456789",
                    NomeFantasia = "Bauducco e Cia Ltda",
                    Uf = "SP",
                    Numero = "2000",
                    Complemento = "Fabrica 1",
                    Bairro = "Guarulhos",
                    Cidade = "Guarulhos",
                    CepFornecedor = 07190000,
                    StatusAtivo = 1,
                    Cnpj = "53495831000149",
                    Rua = "Rod. Pres. Dutra Km 204"
                },
                new Fornecedor
                {
                    InscricaoEstadual = "IE634567890",
                    NomeFantasia = "Danone Ltda",
                    Uf = "MG",
                    Numero = "750",
                    Complemento = "Andar 5",
                    Bairro = "Pocos de Caldas",
                    Cidade = "Pocos de Caldas",
                    CepFornecedor = 37704000,
                    StatusAtivo = 1,
                    Cnpj = "23643315000147",
                    Rua = "Av. Joao Pinheiro"
                }
            };

            foreach (var f in fornecedores)
            {
                if (!await _context.Fornecedores.AnyAsync(x => x.Cnpj == f.Cnpj))
                {
                    await _context.Fornecedores.AddAsync(f);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // CLIENTES (CPFs e CNPJs validos em comprimento)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedClientesAsync()
        {
            var clientes = new[]
            {
                // Pessoa Fisica (CPF = 11 digitos)
                new Cliente { CpfCnpj = "52998224725", TipoCliente = "PessoaFisica" },   // Maria Silva
                new Cliente { CpfCnpj = "18576440076", TipoCliente = "PessoaFisica" },   // Joao Santos
                new Cliente { CpfCnpj = "85296337098", TipoCliente = "PessoaFisica" },   // Ana Oliveira
                new Cliente { CpfCnpj = "73628194053", TipoCliente = "PessoaFisica" },   // Carlos Souza
                new Cliente { CpfCnpj = "41952736084", TipoCliente = "PessoaFisica" },   // Fernanda Lima
                new Cliente { CpfCnpj = "96351478021", TipoCliente = "PessoaFisica" },   // Roberto Almeida
                new Cliente { CpfCnpj = "28463159078", TipoCliente = "PessoaFisica" },   // Patricia Mendes
                // Pessoa Juridica (CNPJ = 14 digitos)
                new Cliente { CpfCnpj = "11222333000181", TipoCliente = "PessoaJuridica" }, // Restaurante Bom Sabor Ltda
                new Cliente { CpfCnpj = "44555666000142", TipoCliente = "PessoaJuridica" }, // Padaria Pao Dourado ME
                new Cliente { CpfCnpj = "77888999000103", TipoCliente = "PessoaJuridica" }, // Lanchonete Central Ltda
            };

            foreach (var c in clientes)
            {
                if (!await _context.Clientes.AnyAsync(x => x.CpfCnpj == c.CpfCnpj))
                {
                    await _context.Clientes.AddAsync(c);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // USUARIOS COMPLEMENTARES
        // (AuthSeeder ja cria: HeitorAdmin, caixa1, fiscal1)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedUsersAsync()
        {
            var templates = new[]
            {
                new { Login = "gerente1", Email = "gerente1@pdv.com", Nome = "Marcos Gerente" },
                new { Login = "caixa2",   Email = "caixa2@pdv.com",   Nome = "Caixa 2 - Julia" },
                new { Login = "caixa3",   Email = "caixa3@pdv.com",   Nome = "Caixa 3 - Rafael" },
                new { Login = "estoque1", Email = "estoque1@pdv.com", Nome = "Pedro Estoquista" },
                new { Login = "compras1", Email = "compras1@pdv.com", Nome = "Lucia Compras" },
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
                        Senha = _passwordService.HashPassword("Pdv@2024"),
                        StatusAtivo = true
                    });
                }
            }
        }

        private async Task SeedUserRolesAsync()
        {
            // Salva primeiro para garantir IDs
            await _context.SaveChangesAsync();

            var roleMap = new Dictionary<string, string>
            {
                { "gerente1", "Manager" },
                { "caixa2",   "Cashier" },
                { "caixa3",   "Cashier" },
                { "estoque1", "StockManager" },
                { "compras1", "Manager" },
            };

            foreach (var (login, roleName) in roleMap)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (user == null || role == null) continue;

                if (!await _context.UserRoles.IgnoreQueryFilters().AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                {
                    await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // PRODUTOS (supermercado real — codigos EAN-13 ficticios)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedProdutosAsync()
        {
            // Salva para garantir IDs de Categoria/Fornecedor
            await _context.SaveChangesAsync();

            var categorias = await _context.Categorias.ToDictionaryAsync(c => c.NomeCategoria, c => c.Id);
            var fornecedores = await _context.Fornecedores.ToDictionaryAsync(f => f.NomeFantasia, f => f.Id);

            // Helper: busca a primeira categoria/fornecedor como fallback
            var fallbackCatId = categorias.Values.FirstOrDefault();
            var fallbackFornId = fornecedores.Values.FirstOrDefault();
            if (fallbackCatId == Guid.Empty || fallbackFornId == Guid.Empty) return;

            Guid Cat(string nome) => categorias.GetValueOrDefault(nome, fallbackCatId);
            Guid Forn(string nome) => fornecedores.FirstOrDefault(f => f.Key.Contains(nome)).Value;

            var produtos = new[]
            {
                // ── BEBIDAS ──
                new Produto
                {
                    CodBarras = "7894900011517", NomeProduto = "Coca-Cola Lata 350ml",
                    DescricaoProduto = "Refrigerante Coca-Cola original lata 350ml",
                    PrecoCusto = 2.80m, PrecoVenda = 4.99m, MargemLucro = 2.19m,
                    DataFabricao = DateTime.UtcNow.AddDays(-15), DataVencimento = DateTime.UtcNow.AddMonths(6),
                    QuantidadeEstoqueProduto = 480, CategoriaId = Cat("Bebidas"), FornecedorId = Forn("Ambev"),
                    StatusAtivo = 1, MinimumStock = 48, MaximumStock = 960, ReorderPoint = 96, Location = "A1-01"
                },
                new Produto
                {
                    CodBarras = "7891991010856", NomeProduto = "Guarana Antarctica 2L",
                    DescricaoProduto = "Refrigerante Guarana Antarctica garrafa 2 litros",
                    PrecoCusto = 4.20m, PrecoVenda = 7.49m, MargemLucro = 3.29m,
                    DataFabricao = DateTime.UtcNow.AddDays(-10), DataVencimento = DateTime.UtcNow.AddMonths(8),
                    QuantidadeEstoqueProduto = 200, CategoriaId = Cat("Bebidas"), FornecedorId = Forn("Ambev"),
                    StatusAtivo = 1, MinimumStock = 24, MaximumStock = 500, ReorderPoint = 50, Location = "A1-02"
                },
                new Produto
                {
                    CodBarras = "7891000315507", NomeProduto = "Agua Mineral Bonafont 500ml",
                    DescricaoProduto = "Agua mineral sem gas 500ml",
                    PrecoCusto = 0.80m, PrecoVenda = 2.49m, MargemLucro = 1.69m,
                    DataFabricao = DateTime.UtcNow.AddDays(-5), DataVencimento = DateTime.UtcNow.AddMonths(12),
                    QuantidadeEstoqueProduto = 600, CategoriaId = Cat("Bebidas"), FornecedorId = Forn("Danone"),
                    StatusAtivo = 1, MinimumStock = 60, MaximumStock = 1200, ReorderPoint = 120, Location = "A1-03"
                },
                new Produto
                {
                    CodBarras = "7891021007016", NomeProduto = "Suco Del Valle Uva 1L",
                    DescricaoProduto = "Suco de uva integral Del Valle caixa 1L",
                    PrecoCusto = 4.50m, PrecoVenda = 8.99m, MargemLucro = 4.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-20), DataVencimento = DateTime.UtcNow.AddMonths(4),
                    QuantidadeEstoqueProduto = 150, CategoriaId = Cat("Bebidas"), FornecedorId = Forn("Ambev"),
                    StatusAtivo = 1, MinimumStock = 20, MaximumStock = 300, ReorderPoint = 40, Location = "A1-04"
                },

                // ── LATICINIOS ──
                new Produto
                {
                    CodBarras = "7891000100103", NomeProduto = "Leite Ninho Integral 1L",
                    DescricaoProduto = "Leite longa vida integral Ninho 1 litro",
                    PrecoCusto = 5.20m, PrecoVenda = 7.99m, MargemLucro = 2.79m,
                    DataFabricao = DateTime.UtcNow.AddDays(-7), DataVencimento = DateTime.UtcNow.AddMonths(3),
                    QuantidadeEstoqueProduto = 300, CategoriaId = Cat("Laticinios"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 36, MaximumStock = 600, ReorderPoint = 72, Location = "B1-01"
                },
                new Produto
                {
                    CodBarras = "7891025110507", NomeProduto = "Iogurte Danone Morango 170g",
                    DescricaoProduto = "Iogurte polpa de morango Danone 170g",
                    PrecoCusto = 2.10m, PrecoVenda = 3.99m, MargemLucro = 1.89m,
                    DataFabricao = DateTime.UtcNow.AddDays(-3), DataVencimento = DateTime.UtcNow.AddDays(21),
                    QuantidadeEstoqueProduto = 180, CategoriaId = Cat("Laticinios"), FornecedorId = Forn("Danone"),
                    StatusAtivo = 1, MinimumStock = 24, MaximumStock = 360, ReorderPoint = 48, Location = "B1-02"
                },
                new Produto
                {
                    CodBarras = "7891025300106", NomeProduto = "Queijo Mussarela Fatiado 150g",
                    DescricaoProduto = "Queijo mussarela fatiado embalagem 150g",
                    PrecoCusto = 6.50m, PrecoVenda = 11.99m, MargemLucro = 5.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-5), DataVencimento = DateTime.UtcNow.AddDays(30),
                    QuantidadeEstoqueProduto = 120, CategoriaId = Cat("Frios e Embutidos"), FornecedorId = Forn("BRF"),
                    StatusAtivo = 1, MinimumStock = 15, MaximumStock = 240, ReorderPoint = 30, Location = "B2-01"
                },

                // ── PADARIA ──
                new Produto
                {
                    CodBarras = "7891962057620", NomeProduto = "Pao de Forma Bauducco 500g",
                    DescricaoProduto = "Pao de forma tradicional Bauducco 500g",
                    PrecoCusto = 5.80m, PrecoVenda = 9.49m, MargemLucro = 3.69m,
                    DataFabricao = DateTime.UtcNow.AddDays(-2), DataVencimento = DateTime.UtcNow.AddDays(15),
                    QuantidadeEstoqueProduto = 100, CategoriaId = Cat("Padaria e Confeitaria"), FornecedorId = Forn("Bauducco"),
                    StatusAtivo = 1, MinimumStock = 15, MaximumStock = 200, ReorderPoint = 30, Location = "C1-01"
                },
                new Produto
                {
                    CodBarras = "7891962036014", NomeProduto = "Biscoito Bauducco Cream Cracker 200g",
                    DescricaoProduto = "Biscoito cream cracker Bauducco 200g",
                    PrecoCusto = 2.90m, PrecoVenda = 5.29m, MargemLucro = 2.39m,
                    DataFabricao = DateTime.UtcNow.AddDays(-10), DataVencimento = DateTime.UtcNow.AddMonths(8),
                    QuantidadeEstoqueProduto = 250, CategoriaId = Cat("Padaria e Confeitaria"), FornecedorId = Forn("Bauducco"),
                    StatusAtivo = 1, MinimumStock = 30, MaximumStock = 500, ReorderPoint = 60, Location = "C1-02"
                },

                // ── FRIOS E EMBUTIDOS ──
                new Produto
                {
                    CodBarras = "7891515901028", NomeProduto = "Presunto Sadia Fatiado 200g",
                    DescricaoProduto = "Presunto cozido fatiado Sadia 200g",
                    PrecoCusto = 7.20m, PrecoVenda = 12.99m, MargemLucro = 5.79m,
                    DataFabricao = DateTime.UtcNow.AddDays(-4), DataVencimento = DateTime.UtcNow.AddDays(25),
                    QuantidadeEstoqueProduto = 90, CategoriaId = Cat("Frios e Embutidos"), FornecedorId = Forn("BRF"),
                    StatusAtivo = 1, MinimumStock = 12, MaximumStock = 180, ReorderPoint = 24, Location = "B2-02"
                },
                new Produto
                {
                    CodBarras = "7891515414313", NomeProduto = "Salsicha Perdigao Hot Dog 500g",
                    DescricaoProduto = "Salsicha hot dog Perdigao pacote 500g",
                    PrecoCusto = 5.30m, PrecoVenda = 9.99m, MargemLucro = 4.69m,
                    DataFabricao = DateTime.UtcNow.AddDays(-6), DataVencimento = DateTime.UtcNow.AddDays(45),
                    QuantidadeEstoqueProduto = 130, CategoriaId = Cat("Frios e Embutidos"), FornecedorId = Forn("BRF"),
                    StatusAtivo = 1, MinimumStock = 15, MaximumStock = 260, ReorderPoint = 30, Location = "B2-03"
                },

                // ── MERCEARIA ──
                new Produto
                {
                    CodBarras = "7891000055304", NomeProduto = "Nescafe Tradicional 200g",
                    DescricaoProduto = "Cafe soluvel Nescafe Tradicional vidro 200g",
                    PrecoCusto = 14.50m, PrecoVenda = 24.99m, MargemLucro = 10.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-30), DataVencimento = DateTime.UtcNow.AddMonths(18),
                    QuantidadeEstoqueProduto = 80, CategoriaId = Cat("Mercearia"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 10, MaximumStock = 160, ReorderPoint = 20, Location = "D1-01"
                },
                new Produto
                {
                    CodBarras = "7891000244005", NomeProduto = "Achocolatado Nescau 400g",
                    DescricaoProduto = "Achocolatado em po Nescau lata 400g",
                    PrecoCusto = 7.80m, PrecoVenda = 13.49m, MargemLucro = 5.69m,
                    DataFabricao = DateTime.UtcNow.AddDays(-20), DataVencimento = DateTime.UtcNow.AddMonths(12),
                    QuantidadeEstoqueProduto = 110, CategoriaId = Cat("Mercearia"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 12, MaximumStock = 220, ReorderPoint = 24, Location = "D1-02"
                },
                new Produto
                {
                    CodBarras = "7891149410200", NomeProduto = "Macarrao Renata Espaguete 500g",
                    DescricaoProduto = "Macarrao espaguete n8 Renata 500g",
                    PrecoCusto = 2.50m, PrecoVenda = 4.79m, MargemLucro = 2.29m,
                    DataFabricao = DateTime.UtcNow.AddDays(-25), DataVencimento = DateTime.UtcNow.AddMonths(10),
                    QuantidadeEstoqueProduto = 200, CategoriaId = Cat("Mercearia"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 24, MaximumStock = 400, ReorderPoint = 48, Location = "D1-03"
                },

                // ── LIMPEZA ──
                new Produto
                {
                    CodBarras = "7891024132104", NomeProduto = "Detergente Ype 500ml",
                    DescricaoProduto = "Detergente liquido lava-loucas Ype 500ml",
                    PrecoCusto = 1.60m, PrecoVenda = 3.49m, MargemLucro = 1.89m,
                    DataFabricao = DateTime.UtcNow.AddDays(-30), DataVencimento = DateTime.UtcNow.AddMonths(24),
                    QuantidadeEstoqueProduto = 300, CategoriaId = Cat("Limpeza"), FornecedorId = Forn("Unilever"),
                    StatusAtivo = 1, MinimumStock = 36, MaximumStock = 600, ReorderPoint = 72, Location = "E1-01"
                },
                new Produto
                {
                    CodBarras = "7891022100303", NomeProduto = "Amaciante Comfort 2L",
                    DescricaoProduto = "Amaciante de roupas Comfort concentrado 2L",
                    PrecoCusto = 10.50m, PrecoVenda = 18.99m, MargemLucro = 8.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-15), DataVencimento = DateTime.UtcNow.AddMonths(24),
                    QuantidadeEstoqueProduto = 100, CategoriaId = Cat("Limpeza"), FornecedorId = Forn("Unilever"),
                    StatusAtivo = 1, MinimumStock = 12, MaximumStock = 200, ReorderPoint = 24, Location = "E1-02"
                },

                // ── HIGIENE PESSOAL ──
                new Produto
                {
                    CodBarras = "7891024035023", NomeProduto = "Sabonete Dove Original 90g",
                    DescricaoProduto = "Sabonete em barra Dove original 90g",
                    PrecoCusto = 2.40m, PrecoVenda = 4.99m, MargemLucro = 2.59m,
                    DataFabricao = DateTime.UtcNow.AddDays(-20), DataVencimento = DateTime.UtcNow.AddMonths(36),
                    QuantidadeEstoqueProduto = 350, CategoriaId = Cat("Higiene Pessoal"), FornecedorId = Forn("Unilever"),
                    StatusAtivo = 1, MinimumStock = 40, MaximumStock = 700, ReorderPoint = 80, Location = "F1-01"
                },
                new Produto
                {
                    CodBarras = "7891150029040", NomeProduto = "Creme Dental Colgate 90g",
                    DescricaoProduto = "Creme dental Colgate Maxima Protecao 90g",
                    PrecoCusto = 3.10m, PrecoVenda = 5.99m, MargemLucro = 2.89m,
                    DataFabricao = DateTime.UtcNow.AddDays(-10), DataVencimento = DateTime.UtcNow.AddMonths(24),
                    QuantidadeEstoqueProduto = 280, CategoriaId = Cat("Higiene Pessoal"), FornecedorId = Forn("Unilever"),
                    StatusAtivo = 1, MinimumStock = 30, MaximumStock = 560, ReorderPoint = 60, Location = "F1-02"
                },

                // ── DOCES E SNACKS ──
                new Produto
                {
                    CodBarras = "7891000305232", NomeProduto = "Chocolate Nestle ao Leite 90g",
                    DescricaoProduto = "Chocolate ao leite Nestle Classic 90g",
                    PrecoCusto = 3.50m, PrecoVenda = 6.99m, MargemLucro = 3.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-12), DataVencimento = DateTime.UtcNow.AddMonths(10),
                    QuantidadeEstoqueProduto = 200, CategoriaId = Cat("Doces e Snacks"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 24, MaximumStock = 400, ReorderPoint = 48, Location = "G1-01"
                },
                new Produto
                {
                    CodBarras = "7891962053004", NomeProduto = "Torrada Bauducco Tradicional 160g",
                    DescricaoProduto = "Torrada levissima tradicional Bauducco 160g",
                    PrecoCusto = 3.20m, PrecoVenda = 5.99m, MargemLucro = 2.79m,
                    DataFabricao = DateTime.UtcNow.AddDays(-8), DataVencimento = DateTime.UtcNow.AddMonths(6),
                    QuantidadeEstoqueProduto = 160, CategoriaId = Cat("Doces e Snacks"), FornecedorId = Forn("Bauducco"),
                    StatusAtivo = 1, MinimumStock = 20, MaximumStock = 320, ReorderPoint = 40, Location = "G1-02"
                },

                // ── CONGELADOS ──
                new Produto
                {
                    CodBarras = "7891515221317", NomeProduto = "Pizza Sadia Mussarela 440g",
                    DescricaoProduto = "Pizza congelada sabor mussarela Sadia 440g",
                    PrecoCusto = 8.50m, PrecoVenda = 15.99m, MargemLucro = 7.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-5), DataVencimento = DateTime.UtcNow.AddMonths(4),
                    QuantidadeEstoqueProduto = 80, CategoriaId = Cat("Congelados"), FornecedorId = Forn("BRF"),
                    StatusAtivo = 1, MinimumStock = 10, MaximumStock = 160, ReorderPoint = 20, Location = "H1-01"
                },
                new Produto
                {
                    CodBarras = "7891515558710", NomeProduto = "Nuggets Perdigao 300g",
                    DescricaoProduto = "Empanado de frango Nuggets Perdigao 300g",
                    PrecoCusto = 7.00m, PrecoVenda = 13.49m, MargemLucro = 6.49m,
                    DataFabricao = DateTime.UtcNow.AddDays(-7), DataVencimento = DateTime.UtcNow.AddMonths(6),
                    QuantidadeEstoqueProduto = 100, CategoriaId = Cat("Congelados"), FornecedorId = Forn("BRF"),
                    StatusAtivo = 1, MinimumStock = 12, MaximumStock = 200, ReorderPoint = 24, Location = "H1-02"
                },

                // ── CEREAIS E GRAOS ──
                new Produto
                {
                    CodBarras = "7891000462508", NomeProduto = "Cereal Nestle Corn Flakes 240g",
                    DescricaoProduto = "Cereal matinal Corn Flakes Nestle 240g",
                    PrecoCusto = 6.80m, PrecoVenda = 12.49m, MargemLucro = 5.69m,
                    DataFabricao = DateTime.UtcNow.AddDays(-15), DataVencimento = DateTime.UtcNow.AddMonths(8),
                    QuantidadeEstoqueProduto = 90, CategoriaId = Cat("Cereais e Graos"), FornecedorId = Forn("Nestle"),
                    StatusAtivo = 1, MinimumStock = 10, MaximumStock = 180, ReorderPoint = 20, Location = "D2-01"
                },
            };

            foreach (var p in produtos)
            {
                // Clear nav props from parameterless constructor to prevent EF from inserting empty entities
                p.Fornecedor = null!;
                p.Categoria = null!;

                if (!await _context.Produtos.AnyAsync(x => x.CodBarras == p.CodBarras))
                {
                    await _context.Produtos.AddAsync(p);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // MOVIMENTACOES DE ESTOQUE (entrada inicial de cada produto)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedStockMovementsAsync()
        {
            await _context.SaveChangesAsync();

            var produtos = await _context.Produtos.ToListAsync();
            foreach (var prod in produtos)
            {
                if (!await _context.StockMovements.AnyAsync(sm => sm.ProdutoId == prod.Id))
                {
                    var sm = new StockMovement
                    {
                        ProdutoId = prod.Id,
                        Quantity = prod.QuantidadeEstoqueProduto,
                        Type = StockMovementType.Entry,
                        Reason = "Estoque inicial - entrada via seed de demonstracao",
                        UnitCost = prod.PrecoCusto,
                        PreviousStock = 0,
                        NewStock = prod.QuantidadeEstoqueProduto,
                        MovementDate = DateTime.UtcNow.AddDays(-30)
                    };
                    // Clear nav prop from constructor (Produto -> Fornecedor/Categoria cascade)
                    sm.Produto = null!;
                    await _context.StockMovements.AddAsync(sm);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // COLABORADORES (vinculados a usuarios e departamentos)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedColaboradoresAsync()
        {
            await _context.SaveChangesAsync();

            var departamentos = await _context.Departamentos.ToDictionaryAsync(d => d.NomeDepartamento, d => d.Id);
            var fallbackDepId = departamentos.Values.FirstOrDefault();

            Guid Dep(string nome) => departamentos.GetValueOrDefault(nome, fallbackDepId);

            var colabData = new[]
            {
                new { Login = "HeitorAdmin", Nome = "Heitor Admin",       Cpf = "12345678901", Cargo = "Diretor Geral",      Dep = "Recursos Humanos",      Tel = "11998765432", EmailP = "heitor@gmail.com" },
                new { Login = "gerente1",    Nome = "Marcos Gerente",      Cpf = "23456789012", Cargo = "Gerente de Loja",     Dep = "Vendas",                Tel = "11997654321", EmailP = "marcos.oliveira@gmail.com" },
                new { Login = "caixa1",      Nome = "Caixa Principal",     Cpf = "34567890123", Cargo = "Operador de Caixa",   Dep = "Vendas",                Tel = "11996543210", EmailP = "caixa.principal@gmail.com" },
                new { Login = "caixa2",      Nome = "Caixa 2 - Julia",     Cpf = "45678901234", Cargo = "Operador de Caixa",   Dep = "Vendas",                Tel = "11995432109", EmailP = "julia.ferreira@gmail.com" },
                new { Login = "caixa3",      Nome = "Caixa 3 - Rafael",    Cpf = "56789012345", Cargo = "Operador de Caixa",   Dep = "Vendas",                Tel = "11994321098", EmailP = "rafael.costa@gmail.com" },
                new { Login = "fiscal1",     Nome = "Fiscal de Caixa",     Cpf = "67890123456", Cargo = "Fiscal de Caixa",     Dep = "Financeiro",            Tel = "11993210987", EmailP = "fiscal@gmail.com" },
                new { Login = "estoque1",    Nome = "Pedro Estoquista",    Cpf = "78901234567", Cargo = "Estoquista",          Dep = "Estoque",               Tel = "11992109876", EmailP = "pedro.estoque@gmail.com" },
                new { Login = "compras1",    Nome = "Lucia Compras",       Cpf = "89012345678", Cargo = "Comprador",           Dep = "Compras",               Tel = "11991098765", EmailP = "lucia.compras@gmail.com" },
            };

            foreach (var c in colabData)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == c.Login);
                if (user == null) continue;

                if (!await _context.Colaboradores.AnyAsync(x => x.Usuario.Id == user.Id))
                {
                    var colab = new Colaborador
                    {
                        NomeColaborador = c.Nome,
                        DepartamentoId = Dep(c.Dep),
                        CpfColaborador = c.Cpf,
                        CargoColaborador = c.Cargo,
                        TelefoneColaborador = c.Tel,
                        EmailPessoalColaborador = c.EmailP,
                        EmailCorporativo = $"{c.Login}@pdv.com",
                        Usuario = user
                    };
                    // Clear empty Departamento from parameterless constructor
                    colab.Departamento = null!;
                    await _context.Colaboradores.AddAsync(colab);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // PEDIDOS COMPLETOS (Pedido + Itens + Cupom + Payment + Fiscal)
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedPedidosCompletoAsync()
        {
            await _context.SaveChangesAsync();

            var clientes = await _context.Clientes.ToListAsync();
            var colaboradores = await _context.Colaboradores.ToListAsync();
            var produtos = await _context.Produtos.ToListAsync();
            if (!clientes.Any() || !colaboradores.Any() || !produtos.Any()) return;

            var existCount = await _context.Pedidos.CountAsync();
            if (existCount >= 10) return; // Ja tem pedidos suficientes

            var formasPagamento = new[] { "Dinheiro", "Cartao de Credito", "Cartao de Debito", "PIX", "Vale Refeicao" };
            var random = new Random(42); // Seed fixo para dados reproduziveis

            // Criar 10 pedidos nos ultimos 7 dias
            for (int i = 0; i < 10; i++)
            {
                var cli = clientes[i % clientes.Count];
                var col = colaboradores[i % colaboradores.Count];
                var formaPag = formasPagamento[i % formasPagamento.Length];
                var dataPedido = DateTime.UtcNow.AddDays(-random.Next(0, 7)).AddHours(-random.Next(1, 12));

                var pedido = new Pedido
                {
                    Status = 1,
                    DataDoPedido = dataPedido,
                    FormaPagamento = formaPag,
                    TotalPedido = 0m,
                    ColaboradorId = col.Id,
                    ClienteId = cli.Id,
                    // Clear nav props from parameterless constructor
                    Colaborador = null,
                    Cliente = null
                };
                await _context.Pedidos.AddAsync(pedido);

                // 2 a 5 itens por pedido
                var itemCount = 2 + (i % 4);
                decimal totalPedido = 0m;
                for (int k = 0; k < itemCount; k++)
                {
                    var prod = produtos[(i * 3 + k) % produtos.Count];
                    var qty = 1 + (k % 4);
                    var itemTotal = prod.PrecoVenda * qty;
                    totalPedido += itemTotal;

                    await _context.ProdutoPedidos.AddAsync(new ProdutoPedido
                    {
                        Pedido = pedido,
                        Produto = prod,
                        CodBarras = prod.CodBarras,
                        QuantidadeItemPedido = qty,
                        TotalProdutoPedido = itemTotal
                    });
                }
                pedido.TotalPedido = totalPedido;

                // Cupom Fiscal
                var serieNum = 1000 + i;
                await _context.Cupoms.AddAsync(new Cupom
                {
                    Pedido = pedido,
                    DataEmissao = dataPedido,
                    NumeroSerie = $"NFC-{serieNum:D6}",
                    ChaveAcesso = $"35{dataPedido:yyMM}{Guid.NewGuid().ToString("N")[..32]}",
                    ValorTotal = totalPedido,
                    DocumentoCliente = cli.CpfCnpj
                });

                // Payment
                var paymentMethod = formaPag switch
                {
                    "Cartao de Credito" => PaymentMethod.CreditCard,
                    "Cartao de Debito" => PaymentMethod.DebitCard,
                    "PIX" => PaymentMethod.Pix,
                    _ => PaymentMethod.Cash
                };

                var payment = new Model.Payment
                {
                    Order = pedido,
                    TotalAmount = totalPedido,
                    Status = PaymentStatus.Processed,
                    PaymentDate = dataPedido
                };
                await _context.Payments.AddAsync(payment);

                // PaymentItem
                await _context.PaymentItems.AddAsync(new PaymentItem
                {
                    Payment = payment,
                    Method = paymentMethod,
                    Amount = totalPedido,
                    Status = PaymentItemStatus.Approved,
                    ProcessedAt = dataPedido
                });

                // PaymentAudit
                var operador = await _context.Usuarios.FirstOrDefaultAsync();
                if (operador != null)
                {
                    await _context.PaymentAudits.AddAsync(new PaymentAudit
                    {
                        Payment = payment,
                        Action = PaymentAuditAction.Processed,
                        Description = $"Pagamento via {formaPag} - R$ {totalPedido:F2}",
                        User = operador,
                        ActionDate = dataPedido
                    });
                }

                // FiscalReceipt (NFC-e) — AccessKey exactly 44 digits
                // Formato: UF(2)+AAMM(4)+CNPJ(14)+mod(2)+serie(3)+nNF(9)+tpEmis(1)+cNF(8)+cDV(1)
                var accessKey = $"35{dataPedido:yyMM}07526557000100650010000{serieNum:D5}1{(10000000 + i):D8}0";
                await _context.FiscalReceipts.AddAsync(new FiscalReceipt
                {
                    Payment = payment,
                    ReceiptNumber = $"{serieNum:D6}",
                    SerialNumber = "001",
                    IssuedAt = dataPedido,
                    Status = FiscalReceiptStatus.Authorized,
                    AccessKey = accessKey
                });
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // SESSOES DE USUARIO
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedUserSessionsAsync()
        {
            var users = await _context.Usuarios.Take(3).ToListAsync();
            if (!users.Any()) return;

            var existCount = await _context.UserSessions.CountAsync();
            if (existCount >= 5) return;

            var ipAddresses = new[] { "192.168.1.10", "192.168.1.11", "192.168.1.12", "192.168.1.20", "10.0.0.5" };
            var userAgents = new[] { "PDV-WinForms/1.0 CAIXA-PC01", "PDV-WinForms/1.0 CAIXA-PC02", "PDV-WinForms/1.0 GERENCIA-PC", "Mozilla/5.0 (Admin)", "PDV-WinForms/1.0 CAIXA-PC03" };

            for (int i = 0; i < 5; i++)
            {
                var user = users[i % users.Count];
                await _context.UserSessions.AddAsync(new UserSession
                {
                    User = user,
                    SessionToken = Guid.NewGuid().ToString("N"),
                    IpAddress = ipAddresses[i],
                    UserAgent = userAgents[i],
                    LoginAt = DateTime.UtcNow.AddHours(-(i + 1) * 2),
                    ExpiresAt = DateTime.UtcNow.AddHours(6),
                    IsActive = i < 3 // 3 sessoes ativas, 2 expiradas
                });
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // LOGS DE AUDITORIA
        // ═══════════════════════════════════════════════════════════════
        private async Task SeedAuditLogsAsync()
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync();
            if (user == null) return;

            var existCount = await _context.AuditLogs.CountAsync();
            if (existCount >= 5) return;

            var auditEntries = new[]
            {
                new { Entity = "Produto", Action = "INSERT", Changes = "{\"NomeProduto\":\"Coca-Cola Lata 350ml\",\"PrecoVenda\":4.99}" },
                new { Entity = "Cliente", Action = "INSERT", Changes = "{\"CpfCnpj\":\"52998224725\",\"TipoCliente\":\"PessoaFisica\"}" },
                new { Entity = "Pedido",  Action = "INSERT", Changes = "{\"TotalPedido\":45.96,\"FormaPagamento\":\"PIX\"}" },
                new { Entity = "Produto", Action = "UPDATE", Changes = "{\"PrecoVenda\":{\"old\":4.50,\"new\":4.99}}" },
                new { Entity = "StockMovement", Action = "INSERT", Changes = "{\"Quantity\":480,\"Type\":\"Entry\",\"Reason\":\"Estoque inicial\"}" },
            };

            for (int i = 0; i < auditEntries.Length; i++)
            {
                var entry = auditEntries[i];
                await _context.AuditLogs.AddAsync(new AuditLog
                {
                    EntityName = entry.Entity,
                    EntityId = Guid.NewGuid(),
                    Action = entry.Action,
                    Changes = entry.Changes,
                    User = user,
                    Timestamp = DateTime.UtcNow.AddMinutes(-i * 15)
                });
            }
        }
    }
}
