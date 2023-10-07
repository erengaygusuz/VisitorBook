using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Core.Models
{
    public class VisitorAddress : BaseModel
    {
        [ValidateNever]
        public Visitor Visitor { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }
        [ValidateNever]
        public State State { get; set; }
    }
}
