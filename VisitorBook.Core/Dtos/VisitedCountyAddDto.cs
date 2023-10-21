using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Dtos
{
    public class VisitedCountyAddDto
    {
        public int VisitorId { get; set; }
        public int CountyId { get; set; }
        public DateTime Date { get; set; }
    }
}
