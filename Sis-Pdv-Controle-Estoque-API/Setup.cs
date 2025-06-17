using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.RemoverCategoria;
using Commands.Departamento.AdicionarDepartamento;
using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Commands.Fornecedor.ListarFornecedor;
using Commands.Fornecedor.ListarFornecedorPorId;
using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.RemoverProduto;
using MediatR;
using Sis_Pdv_Controle_Estoque_API.RabbitMQSender;
using System.Reflection;

namespace Sis_Pdv_Controle_Estoque_API
{
    public static class Setup
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(AdicionarCategoriaRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AdicionarDepartamentoRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AdicionarColaboradorRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AdicionarFornecedorRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AdicionarProdutoRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AdicionarClienteRequest).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(AlterarUsuarioRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AlterarFornecedorRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AlterarProdutoRequest).GetTypeInfo().Assembly);


            services.AddMediatR(typeof(ListarFornecedorRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(ListarFornecedorPorIdRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(ListarFornecedorPorNomeFornecedorRequest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(ListarProdutoPorCodBarrasRequest).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(RemoverCategoriaResquest).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RemoverProdutoResquest).GetTypeInfo().Assembly);
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
