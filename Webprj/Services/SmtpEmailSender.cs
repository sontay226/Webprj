using System.Net.Mail;
using System.Net;
using System.Linq.Expressions;

namespace Webprj.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public SmtpEmailSender( IConfiguration config )
        {
            _config = config;
        }

        public async Task SendEmailAsync( string email , string subject , string htmlMessage )
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = _config["Smtp:Host"] ,
                    Port = int.Parse(_config["Smtp:Port"]) ,
                    EnableSsl = bool.Parse(_config["Smtp:EnableSsl"]) ,
                    Credentials = new NetworkCredential(
                        _config["Smtp:User"] ,
                        _config["Smtp:Pass"])
                };

                var msg = new MailMessage(
                    from: _config["Smtp:From"] ,
                    to: email ,
                    subject: subject ,
                    body: htmlMessage
                )
                { IsBodyHtml = true };

                await smtp.SendMailAsync(msg);
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                throw;
            }
            }
    }
}
