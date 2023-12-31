using VisitorBook.Core.Dtos.VisitorStatisticDtos;

namespace VisitorBook.Core.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public HighestCountOfVisitedCityByVisitorResponseDto GetHighestCountOfVisitedCityByVisitor { get; set; }
        public HighestCountOfVisitedCountyByVisitorResponseDto GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public LongestDistanceByVisitorOneTimeResponseDto GetLongestDistanceByVisitorOneTime { get; set; }
        public LongestDistanceByVisitorAllTimeResponseDto GetLongestDistanceByVisitorAllTime { get; set; }
    }
}
