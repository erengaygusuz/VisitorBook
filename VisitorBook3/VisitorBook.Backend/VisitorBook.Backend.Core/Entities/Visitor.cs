using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Backend.Core.Entities
{
    public class Visitor : BaseEntity
    {
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? VisitorAddressId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorAddressId")]
        public VisitorAddress? VisitorAddress { get; set; }
    }
}
