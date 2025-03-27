using ApplicationLayer.Models;

namespace ApplicationLayer.IProviders
{
    public interface IEmailProvider
    {
        Task<bool> SendEmailAsync(SendNotificationDto request);
    }
}