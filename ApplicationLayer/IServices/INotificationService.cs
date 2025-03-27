using ApplicationLayer.Models;
using DAL.Entities;

namespace ApplicationLayer.IServices
{
    public interface INotificationService
    {
        Notification SendNotification(SendNotificationDto request);
    }
}