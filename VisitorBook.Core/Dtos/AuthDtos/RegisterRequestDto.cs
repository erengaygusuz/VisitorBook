using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.AuthDtos
{
    public class RegisterRequestDto 
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
        [RegularExpression("^((?![0-9]).)*$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [MinLength(5)]
        public string PasswordConfirm { get; set; }
    }
}
