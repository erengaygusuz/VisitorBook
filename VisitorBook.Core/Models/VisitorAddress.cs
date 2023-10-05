using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public class VisitorAddress
    {
        public int Id { get; set; }
        public Visitor Visitor { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
