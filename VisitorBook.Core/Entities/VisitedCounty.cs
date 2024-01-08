using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorBook.Core.Entities
{
    public class VisitedCounty : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime VisitDate { get; set; }        
    }
}
