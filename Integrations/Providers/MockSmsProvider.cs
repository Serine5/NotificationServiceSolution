using Integrations.IProviders;

namespace Integrations.Providers
{
    public class MockSmsProvider : ISmsProvider
    {
        public async Task SendSmsAsync(string recipient, string message)
        {
            await Task.Delay(100);
            Console.WriteLine($"[MockSmsProvider] SMS sent to {recipient} with message: {message}");
        }
    }
}