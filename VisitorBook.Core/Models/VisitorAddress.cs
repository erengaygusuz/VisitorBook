using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class VisitorAddress : BaseModel
    {
        [Display(Name = "Address State")]
        public int StateId { get; set; }
        [Display(Name = "Address City")]
        public int CityId { get; set; }

        public int VisitorId { get; set; }
        [ValidateNever]
        public Visitor Visitor { get; set; }
    }
}
