using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MyClassLibrary.Methods
{
    public class EmailClient
    {
        private IConfiguration _configuration;
        string host;
        int port;
        string username;
        string password;

        public EmailClient(IConfiguration config)
        {
            _configuration = config.GetRequiredSection("Email");
            host = _configuration.GetValue<string>("Host");
            port = _configuration.GetValue<int>("Port");
            username = _configuration.GetValue<string>("Username");
            password = _configuration.GetValue<string>("Password");
        }

        public void Send(string from, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = @$"{body}"
            };

            Send(message);
        }

        public void Send(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(host, port);
                    client.Authenticate(username, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                //TODO Log FailedEmail
            }
        }
    }
}
