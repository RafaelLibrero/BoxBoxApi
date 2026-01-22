using BoxBoxApi.DTOs;
using BoxBoxModels;

namespace BoxBoxApi.Repositories
{
    public interface IRepositoryBoxBox
    {
        Task<User> Register(string userName, string email, string password);
        Task<User> LoginUserAsync(LoginModel loginUser);
        Task<List<User>> GetUsersAsync();
        Task<User> FindUserAsync(int userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);


        Task<List<VTopic>> GetVTopicsAsync();
        Task<Topic> FindTopicAsync(int topicId);
        Task CreateTopicAsync(Topic tema);
        Task UpdateTopicAsync(Topic tema);
        Task DeleteTopicAsync(int topicId);


        Task<ConversationsPaginado> GetVConversationsTopicAsync(int posicion, int topicId);
        Task<Conversation> FindConversationAsync(int conversationId);
        Task<Conversation> CreateConversationAsync(Conversation conversacion);
        Task UpdateConversationAsync(Conversation conversacion);
        Task DeleteConversationAsync(int conversationId);
        Task UpdateEntryCount(int conversationId);


        Task<PostsPaginado> GetPostsConversationAsync(int posicion, int conversationId);
        Task<Post> FindPostAsync(int postId);
        Task<Post> CreatePostAsync(CreatePostDto dto);
        Task UpdatePostAsync(Post posteo);
        Task DeletePostAsync(int postId);
        Task<List<Post>> GetReportedPosts();
        Task ReportPostAsync(int postId);
        Task UnreportPostAsync(int postId);


        Task<List<Driver>> GetDriversAsync();
        Task<Driver> FindDriverAsync(int driverId);
        Task CreateDriverAsync(Driver conductor);
        Task UpdateDriverAsync(Driver conductor);
        Task DeleteDriverAsync(int driverId);


        Task<List<Team>> GetTeamsAsync();
        Task<Team> FindTeamAsync(int teamId);
        Task CreateTeamAsync(Team equipo);
        Task UpdateTeamAsync(Team equipo);
        Task DeleteTeamAsync(int teamId);


        Task<List<Race>> GetRacesAsync();
        Task<Race> FindRaceAsync(int raceId);
        Task CreateRaceAsync(Race carrera);
        Task UpdateRaceAsync(Race carrera);
        Task DeleteRaceAsync(int raceId);
        
    }
}
