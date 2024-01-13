using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.RegisterApplicationDto
{
    public class RegisterApplicationCreateRequestDto
    {
        public int Id { get; set; }

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
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^((?![0-9]).)*$")]
        public string Username { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(500)]
        public string Explanation { get; set; }

        public string Status { get; set; }
    }
}
