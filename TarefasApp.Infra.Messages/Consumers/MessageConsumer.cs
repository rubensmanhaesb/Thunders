using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MessageConsumer(ILogger<MessageConsumer> logger, RabbitMQSettings rabbitMQSettings, EmailService emailService)
        {
            _rabbitMQSettings = rabbitMQSettings;
            _emailService = emailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var connectionFactory = new ConnectionFactory
                    {
                        //Uri = new Uri(_rabbitMQSettings.Url),
                        HostName = _rabbitMQSettings.Host,
                        Port = _rabbitMQSettings.Port,
                        UserName = _rabbitMQSettings.Username,
                        Password = _rabbitMQSettings.Password,
                        VirtualHost = _rabbitMQSettings.VirtualHost
                    };

                    var connection = connectionFactory.CreateConnection();
                    var model = connection.CreateModel();
                    model.QueueDeclare(
                        queue: _rabbitMQSettings.Queue,
                        durable: true,
                        autoDelete: false,
                        exclusive: false,
                        arguments: null
                    );

                    var consumer = new EventingBasicConsumer(model);
                    consumer.Received += (sender, args) =>
                    {
                        try
                        {
                            //ler a mensagem contida na fila
                            var body = Encoding.UTF8.GetString(args.Body.ToArray());

                            //deserializando a mensagem (JSON)
                            var message = JsonConvert.DeserializeObject<EmailMessageModel>(body);

                            //disparando o email..
                            _emailService.SendMail(message);

                            //retirar a mensagem da fila
                            model.BasicAck(args.DeliveryTag, false);
                        }
                        catch (Exception emailEx)
                        {
                            _logger.LogError($"MessageConsumer - Erro ao enviar email: {emailEx.Message}");
                        }
                    };

                    //executando o consumer:
                    model.BasicConsume(_rabbitMQSettings.Queue, false, consumer);
                    _logger.LogInformation("MessageConsumer - Conexão estabelecida com o RabbitMQ.");
                    break; // Sai do loop se a conexão for bem-sucedida
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"MessageConsumer - Error connecting to RabbitMQ: {ex.Message}");
                    _logger.LogError("MessageConsumer - Erro ao conectar ao RabbitMQ. Tentando novamente em 5 segundos...");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}
