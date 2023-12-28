using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Backend.Core.Entities
{
    public class SubRegion : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public int RegionId { get; set; }

        [ValidateNever]
        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}
