namespace BoxBoxApi.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
