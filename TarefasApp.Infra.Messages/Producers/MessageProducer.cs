using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Middleware;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Producers
{
    public class MessageProducer
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<MessageProducer> _logger;
        private readonly IMessageMiddleware _middleware;

        public MessageProducer(ILogger<MessageProducer> logger, RabbitMQSettings rabbitMQSettings, IMessageMiddleware middleware)
        {
            _rabbitMQSettings = rabbitMQSettings;
            _logger = logger;
            _middleware = middleware;
        }

        public async Task SendMessageAsync(EmailMessageModel emailMessageModel, CancellationToken cancellationToken = default)
        {
            await _middleware.ExecuteAsync(async () =>
            {
                using var connection = CreateConnection();
                using var channel = connection.CreateModel();

                DeclareQueue(channel);
                PublishMessage(channel, emailMessageModel);

                _logger.LogInformation("Mensagem enviada com sucesso para o RabbitMQ.");
            }, cancellationToken: cancellationToken);
        }

        private IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Host,
                Port = _rabbitMQSettings.Port,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password,
                VirtualHost = _rabbitMQSettings.VirtualHost
            };

            return factory.CreateConnection();
        }

        private void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(
                queue: _rabbitMQSettings.Queue,
                durable: true,
                autoDelete: false,
                exclusive: false,
                arguments: null
            );
        }

        private void PublishMessage(IModel channel, EmailMessageModel emailMessageModel)
        {
            var messageJson = JsonConvert.SerializeObject(emailMessageModel);
            var messageBody = Encoding.UTF8.GetBytes(messageJson);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _rabbitMQSettings.Queue,
                basicProperties: null,
                body: messageBody
            );
        }
    }

}
