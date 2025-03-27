namespace ApplicationLayer.Models
{
    public class SendNotificationDto
    {
        //[Required]
        //[MinLength(3)]
        public string Channel { get; set; } // "email", "sms", or "push"

        //[Required]
        //[EmailAddress(ErrorMessage = "Recipient must be a valid email for email channel.")]
        public string Recipient { get; set; }

        //[Required]
        //[MaxLength(500)]
        public string Message { get; set; }

        //[Range(1, 10)]
        public int MaxRetries { get; set; } = 3;
    }
}