using System.ComponentModel.DataAnnotations;

namespace thesis_comicverse_webservice_api.DTOs.AuthenticationDTO
{
    public class RegisterDTO
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? repassword { get; set; }

        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? phoneNumber { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? role { get; set; }

    }
}
