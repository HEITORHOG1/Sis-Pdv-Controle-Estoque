using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Map;

namespace Repositories.Base
{
    public class PdvContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public PdvContext(IConfiguration configuration)
        {
            _configuration = configuration;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ControleFluxoCaixaConnectionString");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString, serverVersion)
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
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
        }
    }
}
