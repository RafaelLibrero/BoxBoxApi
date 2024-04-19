using BoxBoxModels;
using Microsoft.EntityFrameworkCore;

namespace BoxBoxApi.Data
{
    public class BoxBoxContext: DbContext
    {
        public BoxBoxContext(DbContextOptions<BoxBoxContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<VTopic> VTopics { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<VConversation> VConversations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Race> Races { get; set; }
    }
}
