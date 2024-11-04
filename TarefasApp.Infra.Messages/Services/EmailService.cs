using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Settings;

namespace TarefasApp.Infra.Messages.Services
{
    public class EmailService 
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public void SendMail(EmailMessageModel model) 
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;  // This line is necessary to avoid SSL/TLS certificate validation

            var mailMessage = new MailMessage(_emailSettings.Email, _emailSettings.To);
            mailMessage.Subject = model.Subject;
            mailMessage.Body = model.Body;

            var smtpClient = new SmtpClient(_emailSettings.Smtp, _emailSettings.Port);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.Send(mailMessage);
        }
    }
}
