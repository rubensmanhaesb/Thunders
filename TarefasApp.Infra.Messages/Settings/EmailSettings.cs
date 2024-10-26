using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Infra.Messages.Settings
{
    public class EmailSettings
    { 
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Smtp { get; set; }
        public int Port { get; set; }
        public string? To { get; set; }
    }
}
