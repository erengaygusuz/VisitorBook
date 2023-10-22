using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class VisitorAddress : BaseModel
    {
        public int VisitorId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorId")]
        public Visitor Visitor { get; set; }

        public int CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }
    }
}
