namespace VisitorBook.Frontend.UI.ViewModels
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionTitle { get; set; }
        public string ExceptionMessage { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
    }
}
