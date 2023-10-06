using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class City : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }

        [JsonIgnore]
        public ICollection<State> States { get; set; }
    }
}
