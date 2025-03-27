using ApplicationLayer.Models;

namespace ApplicationLayer.IProviders
{
    public interface IPushProvider
    {
        Task<bool> SendPushAsync(SendNotificationDto request);
    }
}