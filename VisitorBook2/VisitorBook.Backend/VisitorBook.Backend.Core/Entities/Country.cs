using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Backend.Core.Entities
{
    public class Country : BaseEntity
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
        public int SubRegionId { get; set; }

        [ValidateNever]
        [ForeignKey("SubRegionId")]
        public SubRegion SubRegion { get; set; }
    }
}
