using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Models
{
    public class Visitor : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public Guid? VisitorAddressId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorAddressId")]
        public VisitorAddress? VisitorAddress { get; set; }
    }
}
