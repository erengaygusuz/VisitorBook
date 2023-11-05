using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorBook.Core.Models
{
    public class VisitedCounty : BaseModel
    {
        [Required]
        public Guid VisitorId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorId")]
        public Visitor Visitor { get; set; }

        [Required]
        public Guid CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }        
    }
}
