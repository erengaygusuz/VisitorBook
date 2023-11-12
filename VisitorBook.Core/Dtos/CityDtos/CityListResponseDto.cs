using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Dtos.CityDtos
{
    public class CityListResponseDto
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CityGetResponseDto> Data { get; set; }
    }
}
