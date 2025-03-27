using Integrations.Interfaces;

namespace Integrations.Providers
{
    public class MockEmailProvider : IEmailProvider
    {
        public async Task SendEmailAsync(string recipient, string message)
        {
            await Task.Delay(100);
            Console.WriteLine($"[MockEmailProvider] Email sent to {recipient} with message: {message}");
        }
    }
}