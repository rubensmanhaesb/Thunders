using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Producers
{
    public class MessageProducer
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        public MessageProducer(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public void SendMessage(EmailMessageModel emailMessageModel)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMQSettings.Url)
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

                    //escrever a mensagem na fila
                    model.BasicPublish(
                        exchange: string.Empty,
                        routingKey: _rabbitMQSettings.Queue,
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailMessageModel))
                        );
                }
            }
        }
    }
}
