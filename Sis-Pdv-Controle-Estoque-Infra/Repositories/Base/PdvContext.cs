﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sis_Pdv_Controle_Estoque.Model;
using Sis_Pdv_Controle_Estoque_Infra.Repositories.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Base
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


            base.OnModelCreating(modelBuilder);
        }
    }
}
