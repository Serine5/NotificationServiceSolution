using ApplicationLayer.IProviders;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace NotificationServices
{
    public class NotificationServiceTests
    {
        private readonly Mock<ILogger<NotificationService>> _loggerMock;
        private readonly Mock<INotificationRepository> _repoMock;
        private readonly Mock<IEmailProvider> _mailMock;
        private readonly Mock<ISmsProvider> _smsMock;
        private readonly Mock<IPushProvider> _pushMock;
        private readonly IMemoryCache _memoryCache;

        public NotificationServiceTests()
        {
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _repoMock = new Mock<INotificationRepository>();
            _mailMock = new Mock<IEmailProvider>();
            _smsMock = new Mock<ISmsProvider>();
            _pushMock = new Mock<IPushProvider>();

            _repoMock.Setup(r => r.AddNotification(It.IsAny<Notification>()))
                .Returns<Notification>(n =>
                {
                    n.Id = 1;
                    return n;
                });
            _repoMock.Setup(r => r.UpdateNotification(It.IsAny<Notification>()))
                .Returns<Notification>(n => n);

            // By default, mock mail returns success
            _mailMock.Setup(m => m.SendEmailAsync(It.IsAny<SendNotificationDto>()))
                     .ReturnsAsync(true);

            // Real memory cache
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void SendNotification_ShouldCreateAndSendEmail()
        {
            // Arrange
            var service = new NotificationService(
                _loggerMock.Object,
                _repoMock.Object,
                _mailMock.Object,
                _smsMock.Object,
                _pushMock.Object,
                _memoryCache);

            var request = new SendNotificationDto
            {
                Channel = "email",
                Recipient = "test@example.com",
                Message = "Hello from test",
                MaxRetries = 1
            };

            // Act
            var result = service.SendNotification(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Recipient);
            Assert.True(result.IsSent);
            _repoMock.Verify(r => r.AddNotification(It.IsAny<Notification>()), Times.Once);
            _repoMock.Verify(r => r.UpdateNotification(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public void SendNotification_ShouldRetryIfEmailFailsFirstTime()
        {
            // Arrange
            _mailMock.SetupSequence(m => m.SendEmailAsync(It.IsAny<SendNotificationDto>()))
                .ThrowsAsync(new Exception("Simulated Failure"))
                .ReturnsAsync(true);

            var service = new NotificationService(
                _loggerMock.Object,
                _repoMock.Object,
                _mailMock.Object,
                _smsMock.Object,
                _pushMock.Object,
                _memoryCache);

            var request = new SendNotificationDto
            {
                Channel = "email",
                Recipient = "test@example.com",
                Message = "Test with retry",
                MaxRetries = 2
            };

            // Act
            var result = service.SendNotification(request);

            // Assert
            Assert.True(result.IsSent);
            Assert.Equal(1, result.RetryAttempts); // The first attempt failed, second succeeded
        }

        [Fact]
        public void SendNotification_ShouldThrowIfAllRetriesFail()
        {
            // Arrange
            _mailMock.Setup(m => m.SendEmailAsync(It.IsAny<SendNotificationDto>()))
                .ThrowsAsync(new Exception("Simulated Failure"));

            var service = new NotificationService(
                _loggerMock.Object,
                _repoMock.Object,
                _mailMock.Object,
                _smsMock.Object,
                _pushMock.Object,
                _memoryCache);

            var request = new SendNotificationDto
            {
                Channel = "email",
                Recipient = "test@example.com",
                Message = "This will fail",
                MaxRetries = 2
            };

            // Act & Assert
            Assert.Throws<Exception>(() => service.SendNotification(request));
        }

        [Fact]
        public void SendNotification_ShouldUseCacheToAvoidDuplicate()
        {
            // Arrange
            var service = new NotificationService(
                _loggerMock.Object,
                _repoMock.Object,
                _mailMock.Object,
                _smsMock.Object,
                _pushMock.Object,
                _memoryCache);

            var request = new SendNotificationDto
            {
                Channel = "email",
                Recipient = "test@example.com",
                Message = "Hello",
                MaxRetries = 1
            };

            // First call
            var result1 = service.SendNotification(request);
            Assert.True(result1.IsSent);

            // Second call - should throw because it's cached
            Assert.Throws<InvalidOperationException>(() => service.SendNotification(request));
        }
    }
}