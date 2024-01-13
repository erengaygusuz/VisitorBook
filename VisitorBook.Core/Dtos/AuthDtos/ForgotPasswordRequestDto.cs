using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.AuthDtos
{
    public class ForgotPasswordRequestDto 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
