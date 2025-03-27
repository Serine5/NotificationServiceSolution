using ApplicationLayer.IProviders;
using ApplicationLayer.Models;

namespace Integrations.Providers
{
    public class MockPushProvider : IPushProvider
    {
        public Task<bool> SendPushAsync(SendNotificationDto request)
        {
            Console.WriteLine($"[MOCK PUSH] Sending to {request.Recipient}: {request.Message}");
            // Always returning success in this mock
            return Task.FromResult(true);
        }
    }
}
