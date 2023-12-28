using VisitorBook.Core.Dtos.VisitorStatisticDtos;

namespace VisitorBook.UI.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public HighestCountOfVisitedCityByVisitorResponseDto GetHighestCountOfVisitedCityByVisitor { get; set; }
        public HighestCountOfVisitedCountyByVisitorResponseDto GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public LongestDistanceByVisitorOneTimeResponseDto GetLongestDistanceByVisitorOneTime { get; set; }
        public LongestDistanceByVisitorAllTimeResponseDto GetLongestDistanceByVisitorAllTime { get; set; }
    }
}
