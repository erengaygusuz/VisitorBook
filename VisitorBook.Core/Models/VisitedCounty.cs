﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;


namespace VisitorBook.Core.Models
{
    public class VisitedCounty : BaseModel
    {
        public Guid VisitorId { get; set; }

        [ValidateNever]
        [ForeignKey("VisitorId")]
        public Visitor Visitor { get; set; }

        public Guid CountyId { get; set; }

        [ValidateNever]
        [ForeignKey("CountyId")]
        public County County { get; set; }

        public DateTime VisitDate { get; set; }        
    }
}
