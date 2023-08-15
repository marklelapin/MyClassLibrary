using MimeKit;

namespace MyClassLibrary.Interfaces
{
    public interface IEmailClient
    {
        //Sends an email asynchronously
        public Task<bool> SendAsync(string from, string to, string subject, string body);

        //Sends an email asynchronously
        public Task<bool> SendAsync(MimeMessage message);
    }
}
