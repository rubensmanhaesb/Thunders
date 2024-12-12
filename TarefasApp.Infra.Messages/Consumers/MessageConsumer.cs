using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Middleware;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Services;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Consumers
{
    public class MessageConsumer : BackgroundService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly EmailService _emailService;
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IMessageMiddleware _middleware;
        private IConnection _connection;
        private IModel _channel;

        public MessageConsumer(ILogger<MessageConsumer> logger, RabbitMQSettings rabbitMQSettings, EmailService emailService, IMessageMiddleware middleware)
        {
            _rabbitMQSettings = rabbitMQSettings;
            _emailService = emailService;
            _logger = logger;
            _middleware = middleware;
        }

        private void InitializeRabbitMQ()
        {
            _middleware.ExecuteAsync(async () =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMQSettings.Host,
                    Port = _rabbitMQSettings.Port,
                    UserName = _rabbitMQSettings.Username,
                    Password = _rabbitMQSettings.Password,
                    VirtualHost = _rabbitMQSettings.VirtualHost
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: _rabbitMQSettings.Queue,
                    durable: true,
                    autoDelete: false,
                    exclusive: false,
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (sender, args) => await ProcessMessage(args);
                _channel.BasicConsume(queue: _rabbitMQSettings.Queue, autoAck: false, consumer: consumer);

                _logger.LogInformation("Conexão estabelecida com o RabbitMQ.");
            }).Wait();
        }

        private async Task ProcessMessage(BasicDeliverEventArgs args)
        {
            await _middleware.ExecuteAsync(async () =>
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<EmailMessageModel>(body);

                _emailService.SendMail(message);

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _logger.LogWarning("Tentando conectar ou reconectar ao RabbitMQ...");
                    InitializeRabbitMQ();
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

}
