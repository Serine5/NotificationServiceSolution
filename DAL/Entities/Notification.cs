namespace DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
        public int RetryAttempts { get; set; }
    }
}