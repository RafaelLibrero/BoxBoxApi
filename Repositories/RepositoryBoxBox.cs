using BoxBoxApi.Data;
using BoxBoxModels;

namespace BoxBoxApi.Repositories
{
    public class RepositoryBoxBox : IRepositoryBoxBox
    {
        private BoxBoxContext context;

        public RepositoryBoxBox(BoxBoxContext context)
        {
            this.context = context;
        }

        public Task<Conversation> CreateConversationAsync(Conversation conversacion)
        {
            throw new NotImplementedException();
        }

        public Task CreateDriverAsync(Driver conductor)
        {
            throw new NotImplementedException();
        }

        public Task CreatePostAsync(Post posteo)
        {
            throw new NotImplementedException();
        }

        public Task CreateRaceAsync(Race carrera)
        {
            throw new NotImplementedException();
        }

        public Task CreateTeamAsync(Team equipo)
        {
            throw new NotImplementedException();
        }

        public Task CreateTopicAsync(Topic tema)
        {
            throw new NotImplementedException();
        }

        public Task DeleteConversationAsync(int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDriverAsync(int driverId)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRaceAsync(int raceId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTeamAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTopicAsync(int topicId)
        {
            throw new NotImplementedException();
        }

        public Task<Conversation> FindConversationAsync(int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<Driver> FindDriverAsync(int driverId)
        {
            throw new NotImplementedException();
        }

        public Task<Post> FindPostAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<Race> FindRaceAsync(int raceId)
        {
            throw new NotImplementedException();
        }

        public Task<Team> FindTeamAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> FindTopicAsync(int topicId)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Driver>> GetDriversAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PostsPaginado> GetPostsConversationAsync(int posicion, int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Race>> GetRacesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetReportedPosts()
        {
            throw new NotImplementedException();
        }

        public Task<List<Team>> GetTeamsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ConversationsPaginado> GetVConversationsTopicAsync(int posicion, int topicId)
        {
            throw new NotImplementedException();
        }

        public Task<List<VTopic>> GetVTopicsAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReportPostAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateConversationAsync(Conversation conversacion)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDriverAsync(Driver conductor)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntryCount(int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePostAsync(Post posteo)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRaceAsync(Race carrera)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTeamAsync(Team equipo)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTopicAsync(Topic tema)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
