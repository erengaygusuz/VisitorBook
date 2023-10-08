using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class VisitedState : BaseModel
    {
        [Display(Name = "Visitor")]
        public int VisitorId { get; set; }
        [ValidateNever]
        public Visitor Visitor { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }
        [ValidateNever]
        public State State { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public DateTime Date { get; set; }        
    }
}
