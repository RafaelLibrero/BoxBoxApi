using BoxBoxApi.Data;
using BoxBoxApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BoxBoxApi.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly BoxBoxContext _context;

        public ChatRepository(BoxBoxContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetChatAsync(int chatId)
        {
            return await _context.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<IEnumerable<Chat>> GetUserChatsAsync(int userId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();
        }

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int chatId)
        {
            return await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();
        }

        public async Task<bool> ChatExistsAsync(int user1Id, int user2Id)
        {
            int minUserId = user1Id < user2Id ? user1Id : user2Id;
            int maxUserId = user1Id > user2Id ? user1Id : user2Id;

            return await _context.Chats.AnyAsync(c => c.User1Id == minUserId && c.User2Id == maxUserId);
        }
    }
}
