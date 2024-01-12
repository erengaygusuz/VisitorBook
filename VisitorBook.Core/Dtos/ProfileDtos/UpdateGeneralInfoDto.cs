using Microsoft.AspNetCore.Http;

namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdateGeneralInfoDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
