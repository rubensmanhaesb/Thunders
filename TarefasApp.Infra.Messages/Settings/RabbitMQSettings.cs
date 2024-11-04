using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Infra.Messages.Settings
{
    public class RabbitMQSettings
    { 
        public string? Url { get; set; }
        public string? Queue { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
    }
}
