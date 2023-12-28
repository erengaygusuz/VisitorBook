using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorBook.Backend.Core.Entities
{
    public class VisitorAddress : BaseEntity
    {
        [Required]
        public int CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }
    }
}
