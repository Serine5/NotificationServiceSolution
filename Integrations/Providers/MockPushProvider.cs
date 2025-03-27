using Integrations.IProviders;

namespace Integrations.Providers
{
    public class MockPushProvider : IPushProvider
    {
        public async Task SendPushAsync(string recipient, string message)
        {
            await Task.Delay(100);
            Console.WriteLine($"[MockPushProvider] Push notification sent to {recipient} with message: {message}");
        }
    }
}