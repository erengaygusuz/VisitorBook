using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdateSecurityInfoDto
    {
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string PasswordOld { get; set; }

        [Required]
        [MinLength(5)]
        public string PasswordNew { get; set; }

        [Required]
        [MinLength(5)]
        public string PasswordNewConfirm { get; set; }
    }
}
