using BoxBoxApi.DTOs;
using BoxBoxApi.Models;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BoxBoxApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatsController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatsController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        // GET: api/chats/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatDto>> Get(int id)
        {
            var chat = await _chatRepository.GetChatAsync(id);
            if (chat == null) return NotFound();

            var chatDto = new ChatDto
            {
                Id = chat.Id,
                User1Id = chat.User1Id,
                User2Id = chat.User2Id,
                CreatedAt = chat.CreatedAt,
                Messages = chat.Messages.Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToList()
            };

            return Ok(chatDto);
        }

        // GET: api/chats/user/{userId}
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ChatSummaryDto>>> GetUserChats()
        {
            Claim claimUser = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claimUser.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonUser);
            int userId = user.UserId;

            var chats = await _chatRepository.GetUserChatsAsync(userId);

            var chatDtos = chats.Select(chat => new ChatSummaryDto
            {
                Id = chat.Id,
                User1Id = chat.User1Id,
                User2Id = chat.User2Id,
                CreatedAt = chat.CreatedAt,
                LastMessage = (chat.Messages != null ?
                    chat.Messages
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => new MessageDto
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Content = m.Content,
                            SentAt = m.SentAt
                        })
                        .FirstOrDefault()
                    : null)
            });

            
            return Ok(chatDtos);
        }

        // POST: api/chats
        [HttpPost]
        public async Task<ActionResult<Chat>> Post(ChatCreateDto dto)
        {
            if (await _chatRepository.ChatExistsAsync(dto.User1Id, dto.User2Id))
                return Conflict("Chat between these users already exists.");

            var chat = new Chat
            {
                User1Id = dto.User1Id,
                User2Id = dto.User2Id,
                CreatedAt = DateTime.UtcNow
            };

            await _chatRepository.CreateChatAsync(chat);

            var chatDto = new ChatDto
            {
                Id = chat.Id,
                User1Id = chat.User1Id,
                User2Id = chat.User2Id,
                CreatedAt = chat.CreatedAt,
                Messages = null
            };

            return CreatedAtAction(nameof(Get), new { id = chat.Id }, chatDto);
        }

        // POST: api/chats/{id}/messages
        [HttpPost("{id}/messages")]
        public async Task<ActionResult> AddMessage(int id, MessageCreateDto dto)
        {
            if (id != dto.ChatId) return BadRequest("Chat ID mismatch.");

            var message = new Message
            {
                ChatId = dto.ChatId,
                SenderId = dto.SenderId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow
            };

            await _chatRepository.AddMessageAsync(message);
            return Ok();
        }

        // GET: api/chats/{chatId}/messages
        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int chatId)
        {
            var messages = await _chatRepository.GetMessagesAsync(chatId);

            var messageDtos = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt
            });

            return Ok(messageDtos);

        }
    }
}
