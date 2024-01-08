namespace VisitorBook.Core.Dtos.ExceptionLogDtos
{
    public class ExceptionLogResponseDto
    {
        public int StatusCode { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionTitle { get; set; }

        public string ExceptionMessage { get; set; }

        public string RequestMethod { get; set; }

        public string RequestPath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
