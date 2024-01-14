using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdateGeneralInfoDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression("^((?![0-9]).)*$")]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression("^((?![0-9]).)*$")]
        public string Surname { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
