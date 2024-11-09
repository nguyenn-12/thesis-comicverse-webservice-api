namespace thesis_comicverse_webservice_api.Services
{

    public interface IFileServices
    {
        void WriteLogs(string message);
    }
    public class FileServices : IFileServices
    {
        public void WriteLogs(string message)
        {
            // Write logs to a file
            Console.WriteLine(message);

            string path = "logs.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(message);
            }
        }


    }
}
