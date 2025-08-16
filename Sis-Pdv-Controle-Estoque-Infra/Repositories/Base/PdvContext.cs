using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Map;
using Repositories.Interceptors;

namespace Repositories.Base
{
    public class PdvContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly AuditInterceptor _auditInterceptor;

        public PdvContext(IConfiguration configuration, AuditInterceptor auditInterceptor)
        {
            _configuration = configuration;
            _auditInterceptor = auditInterceptor;
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoPedido> ProdutoPedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cupom> Cupoms { get; set; }
        
        // Authentication entities
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        
        // Payment entities
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
        public DbSet<FiscalReceipt> FiscalReceipts { get; set; }
        public DbSet<PaymentAudit> PaymentAudits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ControleFluxoCaixaConnectionString");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString, serverVersion)
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .AddInterceptors(_auditInterceptor);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //aplicar configurações
            modelBuilder.ApplyConfiguration(new MapCategoria());
            modelBuilder.ApplyConfiguration(new MapCliente());
            modelBuilder.ApplyConfiguration(new MapColaborador());
            modelBuilder.ApplyConfiguration(new MapDepartamento());
            modelBuilder.ApplyConfiguration(new MapFornecedor());
            modelBuilder.ApplyConfiguration(new MapPedido());
            modelBuilder.ApplyConfiguration(new MapProduto());
            modelBuilder.ApplyConfiguration(new MapProdutoPedido());
            modelBuilder.ApplyConfiguration(new MapUsuario());
            modelBuilder.ApplyConfiguration(new MapCupom());
            
            // Authentication configurations
            modelBuilder.ApplyConfiguration(new MapRole());
            modelBuilder.ApplyConfiguration(new MapPermission());
            modelBuilder.ApplyConfiguration(new MapUserRole());
            modelBuilder.ApplyConfiguration(new MapRolePermission());
            modelBuilder.ApplyConfiguration(new MapAuditLog());
            modelBuilder.ApplyConfiguration(new MapUserSession());
            modelBuilder.ApplyConfiguration(new MapStockMovement());
            
            // Payment configurations
            modelBuilder.ApplyConfiguration(new MapPayment());
            modelBuilder.ApplyConfiguration(new MapPaymentItem());
            modelBuilder.ApplyConfiguration(new MapFiscalReceipt());
            modelBuilder.ApplyConfiguration(new MapPaymentAudit());
        }
    }
}
