using System.ComponentModel.DataAnnotations;

namespace thesis_comicverse_webservice_api.DTOs.RequestDTO
{
    public class ComicDTO
    {
        [Required]
        public string? title { get; set; }
        public decimal price { get; set; }
    }
}
