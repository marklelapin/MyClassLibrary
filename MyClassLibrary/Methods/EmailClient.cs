using MailKit.Net.Smtp;
using MailKit.Security;
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
        string fromAddress;

        /// <summary>
        /// Creates an email client to send emails.
        /// </summary>
        /// <remarks>
        /// Appsettings needs the following configuration: <br/>
        /// Email: (e.g.) <br/> 
        /// Host: <br/>
        /// Port: <br/>
        /// Username: <br/>
        /// Password: <br/>
        /// </remarks>

        public EmailClient(IConfiguration config)
        {
            _configuration = config.GetRequiredSection("Email");
            host = _configuration.GetValue<string>("Host");
            port = _configuration.GetValue<int>("Port");
            username = _configuration.GetValue<string>("Username");
            password = _configuration.GetValue<string>("Password");
            fromAddress = _configuration.GetValue<string>("FromAddress");
        }

        public async Task<bool> SendAsync(string from, string to, string subject, string body, string? replyTo = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, fromAddress));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = @$"{body}"
            };

            return await SendAsync(message);

        }

        public async Task<bool> SendAsync(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




    }
}
