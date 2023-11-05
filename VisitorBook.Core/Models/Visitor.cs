using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Models
{
    public class Visitor : BaseModel
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

        public Guid? VisitorAddressId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorAddressId")]
        public VisitorAddress? VisitorAddress { get; set; }
    }
}
