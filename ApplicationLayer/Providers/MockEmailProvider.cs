using ApplicationLayer.IProviders;
using ApplicationLayer.Models;
using System.Text;
using System.Text.Json;

namespace Integrations.Providers
{
    public class MockEmailProvider : IEmailProvider
    {
        private readonly HttpClient _httpClient;

        public MockEmailProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendEmailAsync(SendNotificationDto request)
        {
            // Fake mail payload
            var mailPayload = new
            {
                to = request.Recipient,
                subject = "[MOCK] Notification",
                body = request.Message
            };

            var content = new StringContent(
                JsonSerializer.Serialize(mailPayload),
                Encoding.UTF8,
                "application/json");

            // Fake endpoint
            var response = await _httpClient.PostAsync("https://api.fake-mail.com/send", content);

            return response.IsSuccessStatusCode;
        }
    }
}