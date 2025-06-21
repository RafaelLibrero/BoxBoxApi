using BoxBoxApi.Models;

namespace BoxBoxApi.Repositories
{
    public interface IChatRepository
    {
        Task<Chat> GetChatAsync(int chatId);
        Task<IEnumerable<Chat>> GetUserChatsAsync(int userId);
        Task<Chat> CreateChatAsync(Chat chat);
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(int chatId);
        Task<bool> ChatExistsAsync(int user1Id, int user2Id);
    }
}
