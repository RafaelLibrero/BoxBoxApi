using BoxBoxModels;

namespace BoxBoxApi.Repositories
{
    public interface IRepositoryBoxBox
    {
        Task<Conversation> CreateConversationAsync(Conversation conversacion);
        Task CreateDriverAsync(Driver conductor);
        Task CreatePostAsync(Post posteo);
        Task CreateRaceAsync(Race carrera);
        Task CreateTeamAsync(Team equipo);
        Task CreateTopicAsync(Topic tema);
        Task DeleteConversationAsync(int conversationId);
        Task DeleteDriverAsync(int driverId);
        Task DeletePostAsync(int postId);
        Task DeleteRaceAsync(int raceId);
        Task DeleteTeamAsync(int teamId);
        Task DeleteTopicAsync(int topicId);
        Task<Conversation> FindConversationAsync(int conversationId);
        Task<Driver> FindDriverAsync(int driverId);
        Task<Post> FindPostAsync(int postId);
        Task<Race> FindRaceAsync(int raceId);
        Task<Team> FindTeamAsync(int teamId);
        Task<Topic> FindTopicAsync(int topicId);
        Task<User> FindUserAsync(int userId);
        Task<List<Driver>> GetDriversAsync();
        Task<PostsPaginado> GetPostsConversationAsync(int posicion, int conversationId);
        Task<List<Race>> GetRacesAsync();
        Task<List<Post>> GetReportedPosts();
        Task<List<Team>> GetTeamsAsync();
        Task<ConversationsPaginado> GetVConversationsTopicAsync(int posicion, int topicId);
        Task<List<VTopic>> GetVTopicsAsync();
        Task ReportPostAsync(int postId);
        Task UpdateConversationAsync(Conversation conversacion);
        Task UpdateDriverAsync(Driver conductor);
        Task UpdateEntryCount(int conversationId);
        Task UpdatePostAsync(Post posteo);
        Task UpdateRaceAsync(Race carrera);
        Task UpdateTeamAsync(Team equipo);
        Task UpdateTopicAsync(Topic tema);
        Task UpdateUserAsync(User user);
    }
}
