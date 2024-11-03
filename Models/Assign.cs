namespace thesis_comicverse_webservice_api.Models
{
    public class Assign
    {
        public int AssignID { get; set; }
        public int userId { get; set; }
        public int TaskID { get; set; }
        public DateTime? assignAt { get; set; }

    }
}
