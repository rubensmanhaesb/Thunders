using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Consumers;
using TarefasApp.Infra.Messages.Middleware;
using TarefasApp.Infra.Messages.Producers;
using TarefasApp.Infra.Messages.Services;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Extensions
{
    public static class RabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            #region Definições para acesso ao RabbitMQ

            var rabbitMQSettings = new RabbitMQSettings();

            new ConfigureFromConfigurationOptions<RabbitMQSettings>
                (configuration.GetSection("RabbitMQ")).Configure(rabbitMQSettings);

            services.AddSingleton(rabbitMQSettings);

            #endregion

            #region Definições para acesso ao servidor de emails

            var emailSettings = new EmailSettings();

            new ConfigureFromConfigurationOptions<EmailSettings>
                (configuration.GetSection("Mail")).Configure(emailSettings);

            services.AddSingleton(emailSettings);

            #endregion

            services.AddTransient<MessageProducer>();
            services.AddTransient<EmailService>();
            services.AddHostedService<MessageConsumer>();
            services.AddTransient<IMessageMiddleware, MessageMiddleware>();

            return services;
        }
    }
}
