using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MyClassLibrary.Interfaces;

namespace MyClassLibrary.Email
{
    public class HotmailClient : IEmailClient
    {
        private IConfiguration _configuration;
        string host;
        int port;
        string username;
        string password;
        string fromAddress;
        string toAddress;

        /// <summary>
        /// Creates an email client to send emails using hotmail.
        /// </summary>
        /// <remarks>
        /// Appsettings needs the following configuration: <br/>
        /// Email: (e.g.) <br/> 
        /// Username: <br/>
        /// Password: <br/>
        /// FromAddress: "magcarter@hotmail.co.uk"
        /// </remarks>

        public HotmailClient(IConfiguration config)
        {
            _configuration = config.GetRequiredSection("Email");
            host = "smtp-mail.outlook.com";
            port = 587;
            username = _configuration.GetValue<string>("Username");
            password = _configuration.GetValue<string>("Password");
            fromAddress = _configuration.GetValue<string>("FromAddress");
            toAddress = _configuration.GetValue<string>("ToAddress");
        }

        public async Task<bool> SendAsync(string from, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, fromAddress));
            if (to == "default")
            {
                message.To.Add(new MailboxAddress(toAddress, toAddress));
            }
            else
            {
                message.To.Add(new MailboxAddress(to, to));
            }

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
