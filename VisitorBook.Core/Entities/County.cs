using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VisitorBook.Core.Attributes;

namespace VisitorBook.Core.Entities
{
    public class County : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }

        [Required]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid CityId { get; set; }

        [ValidateNever]
        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
