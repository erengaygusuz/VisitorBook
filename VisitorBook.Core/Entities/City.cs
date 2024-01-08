using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorBook.Core.Entities
{
    public class City : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Code { get; set; }

        [Required]
        public int CountryId { get; set; }

        [ValidateNever]
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
