namespace Integrations.IProviders
{
    public interface ISmsProvider
    {
        Task SendSmsAsync(string recipient, string message);
    }
}