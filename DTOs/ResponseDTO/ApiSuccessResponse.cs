namespace thesis_comicverse_webservice_api.DTOs.ResponseDTO
{
    public class ApiSuccessResponse
    {
        public string? status { get; set; }
        public dynamic? message { get; set; }

        public ApiSuccessResponse(string status, dynamic message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
