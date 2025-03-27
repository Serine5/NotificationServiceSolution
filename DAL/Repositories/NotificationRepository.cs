using DAL.Context;
using DAL.Entities;

namespace DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationsDbContext _context;

        public NotificationRepository(NotificationsDbContext context)
        {
            _context = context;
        }

        public Notification AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification? GetNotification(int id)
        {
            return _context.Notifications.Find(id);
        }
    }
}
