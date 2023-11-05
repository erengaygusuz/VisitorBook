using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitorBook.Core.Attributes;

namespace VisitorBook.Core.Models
{
    public class VisitorAddress : BaseModel
    {
        [Required]
        [NotEmptyGuid]
        public Guid CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }
    }
}
