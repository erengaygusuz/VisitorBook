using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Core.Dtos.UserDtos
{
    public class UserRequestDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        [ValidateNever]
        public string SecurityStamp { get; set; }
    }
}
