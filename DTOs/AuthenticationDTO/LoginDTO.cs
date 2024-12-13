using System.ComponentModel.DataAnnotations;

namespace thesis_comicverse_webservice_api.DTOs.AuthenticationDTO
{
    public class LoginDTO
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
