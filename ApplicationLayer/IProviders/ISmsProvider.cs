using ApplicationLayer.Models;

namespace ApplicationLayer.IProviders
{
    public interface ISmsProvider
    {
        Task<bool> SendSmsAsync(SendNotificationDto request);
    }
}