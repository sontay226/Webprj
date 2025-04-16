namespace Webprj.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email , string subject , string htmlMessgae);
    }
}
