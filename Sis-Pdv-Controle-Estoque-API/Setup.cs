using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.RemoverCategoria;
using Commands.Cliente.AdicionarCliente;
using Commands.Colaborador.AdicionarColaborador;
using Commands.Departamento.AdicionarDepartamento;
using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Commands.Fornecedor.ListarFornecedor;
using Commands.Fornecedor.ListarFornecedorPorId;
using Commands.Fornecedor.ListarFornecedorPorNomeDepartamento;
using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.ListarProdutoPorNomeProduto;
using Commands.Produto.RemoverProduto;
using Commands.Usuarios.AlterarUsuario;
using Interfaces;
using MediatR;
using Repositories;
using Repositories.Base;
using Repositories.Transactions;
using Sis_Pdv_Controle_Estoque_API.RabbitMQSender;
using System.Reflection;

namespace Sis_Pdv_Controle_Estoque_API
{
    public static class Setup
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(AdicionarCategoriaRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarDepartamentoRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarColaboradorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarProdutoRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarClienteRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(AlterarUsuarioRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AlterarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AlterarProdutoRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorPorIdRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorPorNomeFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarProdutoPorCodBarrasRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(RemoverCategoriaResquest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RemoverProdutoResquest).GetTypeInfo().Assembly);
            });
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<PdvContext, PdvContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IRepositoryCategoria, RepositoryCategoria>();

            services.AddTransient<IRepositoryDepartamento, RepositoryDepartamento>();

            services.AddTransient<IRepositoryColaborador, RepositoryColaborador>();

            services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();

            services.AddTransient<IRepositoryFornecedor, RepositoryFornecedor>();

            services.AddTransient<IRepositoryProduto, RepositoryProduto>();

            services.AddTransient<IRepositoryCliente, RepositoryCliente>();

            services.AddTransient<IRepositoryPedido, RepositoryPedido>();

            services.AddTransient<IRepositoryProdutoPedido, RepositoryProdutoPedido>();

            services.AddTransient<IRabbitMQMessageSender, RabbitMQMessageSender>();
        }
    }
}
