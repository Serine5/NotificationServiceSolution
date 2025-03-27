using ApplicationLayer.IServices;
using ApplicationLayer.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace NotificationServiceSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IValidator<SendNotificationDto> _validator;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationService notificationService,
            IValidator<SendNotificationDto> validator,
            ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost("send")]
        public IActionResult SendNotification([FromBody] SendNotificationDto request)
        {
            ValidationResult result = _validator.Validate(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var notification = _notificationService.SendNotification(request);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification.");
                return StatusCode(500, "Failed to send notification.");
            }
        }
    }
}