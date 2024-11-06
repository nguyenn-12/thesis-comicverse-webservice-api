using Microsoft.AspNetCore.Mvc;

namespace thesis_comicverse_webservice_api.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
