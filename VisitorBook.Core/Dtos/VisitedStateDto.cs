using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Dtos
{
    public class VisitedStateDto
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int StateId { get; set; }
        public DateTime Date { get; set; }
    }
}
