namespace thesis_comicverse_webservice_api.Models
{
    public class Task
    {
        public int taskID { get; set; }
        public string? taskName { get; set; }
        public DateTime? createAt { get; set; }
        public DateTime? deadline { get; set; }
        public decimal? progressPercentage { get; set; }
        public string? priority { get; set; }
        public string? status { get; set; }

    }
}
