using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitorBook.Core.Attributes;

namespace VisitorBook.Core.Models
{
    public class VisitedCounty : BaseModel
    {
        [Required]
        [NotEmptyGuid]
        public Guid VisitorId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorId")]
        public Visitor Visitor { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime VisitDate { get; set; }        
    }
}
