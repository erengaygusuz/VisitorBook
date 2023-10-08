using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Core.Models
{
    public class State : BaseModel
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }
        [ValidateNever]
        public City City { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public List<Visitor> Visitors { get; set; }
    }
}
