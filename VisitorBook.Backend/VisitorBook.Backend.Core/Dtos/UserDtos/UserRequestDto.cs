using VisitorBook.Backend.Core.Enums;

namespace VisitorBook.Backend.Core.Dtos.UserDtos
{
    public class UserRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
