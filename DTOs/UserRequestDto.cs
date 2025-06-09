namespace BoxBoxApi.DTOs
{
    public class UserRequestDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; } 
        public string? ProfilePicture { get; set; }
        public int? TeamId { get; set; }
        public int? DriverId { get; set; }
        public string? Biography { get; set; }
    }
}
