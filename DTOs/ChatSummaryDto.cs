namespace BoxBoxApi.DTOs
{
    public class ChatSummaryDto
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageDto LastMessage { get; set; }
    }
}
