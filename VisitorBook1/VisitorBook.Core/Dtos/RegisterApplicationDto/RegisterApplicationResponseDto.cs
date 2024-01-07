using VisitorBook.Core.Dtos.UserDtos;

namespace VisitorBook.Core.Dtos.RegisterApplicationDto
{
    public class RegisterApplicationResponseDto
    {
        public int Id { get; set; }

        public UserResponseDto User { get; set; }

        public string Explanation { get; set; }

        public string Status { get; set; }
    }
}
