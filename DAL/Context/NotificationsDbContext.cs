using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Recipient).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Channel).IsRequired();
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsSent)
                      .HasDefaultValue(false);
                entity.Property(e => e.SentAt);
                entity.Property(e => e.RetryAttempts);
            });
        }

    }
}