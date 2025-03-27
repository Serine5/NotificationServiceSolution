using DAL.Entities;

namespace DAL.Repositories
{
    public interface INotificationRepository
    {
        Notification AddNotification(Notification notification);
        Notification UpdateNotification(Notification notification);
        Notification? GetNotification(int id);
    }
}
