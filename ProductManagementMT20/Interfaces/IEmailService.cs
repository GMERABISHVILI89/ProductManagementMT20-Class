namespace ProductManagementMT20.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string to, string subject, string htmlMessage);
    }
}
