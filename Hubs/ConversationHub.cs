using Microsoft.AspNetCore.SignalR;

namespace BoxBoxApi.Hubs
{
    public class ConversationHub : Hub
    {

        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

        public async Task LeaveConversation(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

    }
}
