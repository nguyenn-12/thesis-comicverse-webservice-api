using System.Net;

namespace thesis_comicverse_webservice_api.DTOs.ExceptionDTO
{
    public class ExceptionResponse
    {
        public string Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Description { get; set; }

        public ExceptionResponse(HttpStatusCode statusCode, string description)
        {
            Status = "Error";
            StatusCode = statusCode;
            Description = description;
        }
    }
}
