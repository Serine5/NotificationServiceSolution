namespace Integrations.Interfaces
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(string recipient, string message);
    }
}