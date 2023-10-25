
using System.Text.Json;

namespace VisitorBook.Core.Models
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionTitle { get; set; }
        public string ExceptionMessage { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
