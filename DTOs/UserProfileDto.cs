namespace BoxBoxApi.DTOs
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; } 
        public string ProfilePicture { get; set; }
        public int TotalPosts { get; set; }
        public int? TeamId { get; set; }
        public int? DriverId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastAccess { get; set; }
        public string? Biography { get; set; }
        public string Name { get; set; }
    }
}
