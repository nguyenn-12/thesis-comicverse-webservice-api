namespace thesis_comicverse_webservice_api.Models
{
    public class User
    {
        public int userId { get; set; }
        public string? userName { get; set; }
        public string? email { get; set; }
        public string? hashedPassword { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? phoneNumber { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? status { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? lastLogin { get; set; }
        public string? role { get; set; }
    }
}
