using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
namespace Webprj.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public MailKitEmailSender( IConfiguration config )
        {
            _config = config;
        }

        public async Task SendEmailAsync( string toEmail , string subject , string htmlMessage )
        {
            Console.WriteLine("Thuc thi gui email");
            var mailSettings = _config.GetSection("MailSettings");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(mailSettings["SenderName"] , mailSettings["SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            // Console.WriteLine("thong tin tin nhan {}, {}, {}" , message.Subject , message.Body); ;
            try
            {
                using var smtp = new SmtpClient(new ProtocolLogger("smtp-log.txt"));
                await smtp.ConnectAsync(
                    mailSettings["SmtpServer"] ,
                    int.Parse(mailSettings["SmtpPort"]) ,
                    MailKit.Security.SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    mailSettings["Username"] ,
                    mailSettings["Password"]);

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                Console.WriteLine(message.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== MAILKIT EXCEPTION ===");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}