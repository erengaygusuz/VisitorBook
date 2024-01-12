using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [ValidateNever]
        public UserAddress? UserAddress { get; set; }

        public string? Picture { get; set;  }
    }
}
