using ApplicationLayer.Models;
using FluentValidation;

namespace ApplicationLayer.Validators
{
    public class SendNotificationDtoValidator : AbstractValidator<SendNotificationDto>
    {
        public SendNotificationDtoValidator()
        {
            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .MinimumLength(3).WithMessage("Channel must be at least 3 characters (e.g. 'email').");

            RuleFor(x => x.Recipient)
                .NotEmpty().WithMessage("Recipient is required.")
                .EmailAddress().When(x => x.Channel.Equals("email", System.StringComparison.OrdinalIgnoreCase))
                .WithMessage("Recipient must be a valid email if channel is 'email'.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message cannot be empty.")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.");

            RuleFor(x => x.MaxRetries)
                .GreaterThanOrEqualTo(1).WithMessage("MaxRetries must be at least 1.")
                .LessThanOrEqualTo(10).WithMessage("MaxRetries cannot exceed 10.");
        }
    }
}
