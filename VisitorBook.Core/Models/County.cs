using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VisitorBook.Core.Models
{
    public class County : BaseModel
    {
        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int CityId { get; set; }

        [ValidateNever]
        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
