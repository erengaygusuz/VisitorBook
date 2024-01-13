using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.ContactMessageDtos
{
    public class ContactMessageRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression("^((?![0-9]).)*$")]
        public string NameSurname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Subject { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(500)]
        public string Message { get; set; }
    }
}
