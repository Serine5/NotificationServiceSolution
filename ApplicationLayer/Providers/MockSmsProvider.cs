using ApplicationLayer.IProviders;
using ApplicationLayer.Models;

namespace Integrations.Providers
{
    public class MockSmsProvider : ISmsProvider
    {
        public Task<bool> SendSmsAsync(SendNotificationDto request)
        {
            Console.WriteLine($"[MOCK SMS] Sending to {request.Recipient}: {request.Message}");
            // Always returning success in this mock
            return Task.FromResult(true);
        }
    }
}