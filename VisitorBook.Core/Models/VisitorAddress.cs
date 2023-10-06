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
        public Visitor Visitor { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }
        public State State { get; set; }
    }
}
