using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        [ValidateNever]
        public VisitorAddress? VisitorAddress { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public List<State> States { get; set; }
    }
}
