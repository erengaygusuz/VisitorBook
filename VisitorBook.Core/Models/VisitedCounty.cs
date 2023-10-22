using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class VisitedCounty : BaseModel
    {
        public int VisitorAddressId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorAddressId")]
        public VisitorAddress VisitorAddress { get; set; }

        public int CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
