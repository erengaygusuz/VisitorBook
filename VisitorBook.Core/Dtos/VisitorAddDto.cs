
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Dtos
{
    public class VisitorAddDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
