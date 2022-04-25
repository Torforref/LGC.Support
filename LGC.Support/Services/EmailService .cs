using LGC.Support.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace LGC.Support.Services
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }

    public class EmailService : IEmailService
    {
        public void Send(string from, string to, string subject, string html)
        {

           // var SmtpHost = "smtp.live.com";  // hotmail
           // var SmtpHost = "smtp.office365.com";  // office 365
            var SmtpHost = "smtp.gmail.com";  // gmail
            var SmtpPort = 587;


            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;    
            html += "<br /><br /> <b>Logicode Co., Ltd.</ b>";
            var html_text = $"<font color='black'>{html}</font>";
            email.Body = new TextPart(TextFormat.Html) { Text = html_text };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            var password = config["MyConfig:EmailPassword"];
            smtp.Authenticate(from, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
