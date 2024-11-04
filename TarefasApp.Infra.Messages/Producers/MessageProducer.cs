using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Consumers;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Producers
{
    public class MessageProducer
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<MessageProducer> _logger;

        public MessageProducer(ILogger<MessageProducer> logger, RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
            _logger = logger;
        }

        public void SendMessage(EmailMessageModel emailMessageModel)
        {
            int retryCount = 5; // Número máximo de tentativas
            int delayBetweenRetries = 5000; // Tempo em milissegundos entre as tentativas (5 segundos)

            for (int attempt = 1; attempt <= retryCount; attempt++)
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

                    using (var connection = connectionFactory.CreateConnection())
                    {

                        using (var model = connection.CreateModel())
                        {

                            model.QueueDeclare(
                                queue: _rabbitMQSettings.Queue,
                                durable: true,
                                autoDelete: false,
                                exclusive: false,
                                arguments: null
                            );

                            var json = JsonConvert.SerializeObject(emailMessageModel);
                            var body = Encoding.UTF8.GetBytes(json);

                            //escrever a mensagem na fila
                            model.BasicPublish(
                                exchange: string.Empty,
                                routingKey: _rabbitMQSettings.Queue,
                                basicProperties: null,
                                body: body // Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailMessageModel))
                                );

                            _logger.LogInformation($"MessageProducer - Conexão realizada com sucesso RabbitMQ: ");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"MessageProducer - Error connecting to RabbitMQ: {ex.Message}");
                    _logger.LogError($"MessageProducer - Erro ao conectar ao RabbitMQ. Tentando novamente em {delayBetweenRetries} milissegundos...");

                    if (attempt == retryCount)
                    {
                        _logger.LogError("MessageProducer - Máximo de tentativas atingido. Não foi possível conectar ao RabbitMQ.");
                    }
                    else
                    {
                        _logger.LogInformation("MessageProducer - Aguradando o próximo send para tentarnovamente...");
                        // Aguardar antes de tentar novamente
                        Thread.Sleep(delayBetweenRetries);
                    }
                }
            }
        }
    }
}
