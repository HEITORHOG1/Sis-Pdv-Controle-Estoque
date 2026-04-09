
using MassTransit;
using Sis_Pdv_Controle_Estoque_API.Configuration;
using Sis_Pdv_Controle_Estoque_API.Messaging.Consumers;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public static class MessagingConfiguration
{
    public static IServiceCollection AddMessagingConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection(RabbitMQOptions.SectionName);
        var config = section.Get<RabbitMQOptions>();

        // Se não tiver config, não sobe messaging (pode ser dev sem rabbit)
        // Mas em produção é crítico.
        if (config == null)
        {
            // Fallback ou throw?
            // Melhor não crashar se estiver opcional, mas vamos assumir que precisa.
            return services;
        }

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            // Consumers
            x.AddConsumer<VendaRealizadaConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.HostName, config.VirtualHost, h =>
                {
                    h.Username(config.UserName);
                    h.Password(config.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
