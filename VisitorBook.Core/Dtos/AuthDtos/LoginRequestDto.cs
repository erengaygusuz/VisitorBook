using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.AuthDtos
{
    public class LoginRequestDto 
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
