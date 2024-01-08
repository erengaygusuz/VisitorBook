using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Entities
{
    public class RegisterApplication : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string Explanation { get; set; }

        public RegisterApplicationStatus Status { get; set; }
    }
}
