using ApplicationLayer.IProviders;
using ApplicationLayer.IServices;
using ApplicationLayer.Models;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Services
{
    public class NotificationService : INotificationService
    {

        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailProvider _mailIntegration;
        private readonly ISmsProvider _smsIntegration;
        private readonly IPushProvider _pushIntegration;
        private readonly IMemoryCache _memoryCache;

        public NotificationService(
            ILogger<NotificationService> logger,
            INotificationRepository notificationRepository,
            IEmailProvider mailIntegration,
            ISmsProvider smsIntegration,
            IPushProvider pushIntegration,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _mailIntegration = mailIntegration;
            _smsIntegration = smsIntegration;
            _pushIntegration = pushIntegration;
            _memoryCache = memoryCache;
        }

        public Notification SendNotification(SendNotificationDto request)
        {
            // Check the in-memory cache to avoid duplicates
            var cacheKey = $"SendNotification-{request.Channel}-{request.Recipient}-{request.Message}";
            if (_memoryCache.TryGetValue(cacheKey, out _))
            {
                _logger.LogInformation("Duplicate request detected via cache; skipping.");
                throw new InvalidOperationException("Notification recently sent.");
            }

            var notification = new Notification
            {
                Recipient = request.Recipient,
                Message = request.Message,
                Channel = request.Channel,
                CreatedAt = DateTime.UtcNow,
                IsSent = false,
                RetryAttempts = 0
            };

            notification = _notificationRepository.AddNotification(notification);

            bool isSuccessful = false;
            Exception? lastException = null;

            for (int attempt = 1; attempt <= request.MaxRetries; attempt++)
            {
                try
                {
                    SendThroughChannelAsync(request).GetAwaiter().GetResult();
                    isSuccessful = true;
                    break;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    notification.RetryAttempts = attempt;
                    _logger.LogError(ex,
                        $"Failed sending by {request.Channel}, attempt {attempt}/{request.MaxRetries}");
                }
            }

            if (!isSuccessful && lastException != null)
            {
                _logger.LogError(lastException, "All retry attempts failed.");
                notification.IsSent = false;
                _notificationRepository.UpdateNotification(notification);
                throw lastException;
            }

            // make as sent
            notification.IsSent = true;
            notification.SentAt = DateTime.UtcNow;
            notification = _notificationRepository.UpdateNotification(notification);

            // Cache this request for 2 minutes to prevent duplicates
            _memoryCache.Set(cacheKey, true, TimeSpan.FromMinutes(2));

            return notification;
        }

        private async Task SendThroughChannelAsync(SendNotificationDto request)
        {
            switch (request.Channel.ToLower())
            {
                case "email":
                    if (!await _mailIntegration.SendEmailAsync(request))
                        throw new Exception("Email sending failed (integration).");
                    break;

                case "sms":
                    if (!await _smsIntegration.SendSmsAsync(request))
                        throw new Exception("SMS sending failed (integration).");
                    break;

                case "push":
                    if (!await _pushIntegration.SendPushAsync(request))
                        throw new Exception("Push sending failed (integration).");
                    break;

                default:
                    throw new NotImplementedException($"Channel '{request.Channel}' not supported.");
            }
        }
    }
}