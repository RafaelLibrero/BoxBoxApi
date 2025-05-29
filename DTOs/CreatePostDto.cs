namespace BoxBoxApi.DTOs
{
    public class CreatePostDto
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
