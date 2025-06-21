namespace BoxBoxApi.DTOs
{
    public class MessageCreateDto
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
    }
}
