namespace Integrations.IProviders
{
    public interface IPushProvider
    {
        Task SendPushAsync(string recipient, string message);
    }
}