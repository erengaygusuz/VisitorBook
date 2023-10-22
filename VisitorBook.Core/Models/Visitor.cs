using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Core.Models
{
    public enum Gender
    {
        Man,
        Woman
    }

    public class Visitor : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public int? VisitorAddressId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorAddressId")]
        public VisitorAddress? VisitorAddress { get; set; }
    }
}
