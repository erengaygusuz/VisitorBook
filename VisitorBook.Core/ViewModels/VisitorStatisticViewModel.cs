using VisitorBook.Core.Dtos.UserDataStatisticDtos;

namespace VisitorBook.Core.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public HighestCountOfVisitedCountryByVisitorResponseDto GetHighestCountOfVisitedCountryByVisitor { get; set; }
        public HighestCountOfVisitedCityByVisitorResponseDto GetHighestCountOfVisitedCityByVisitor { get; set; }
        public HighestCountOfVisitedCountyByVisitorResponseDto GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public LongestDistanceByVisitorOneTimeResponseDto GetLongestDistanceByVisitorOneTime { get; set; }
        public LongestDistanceByVisitorAllTimeResponseDto GetLongestDistanceByVisitorAllTime { get; set; }

        public int GetTotalUserCount { get; set; }
        public int GetTotalVisitorUserCount { get; set; }
        public int GetTotalVisitorRecorderUserCount { get; set; }
        public int GetTotalAdminUserCount { get; set; }

        public int GetTotalRegionCount { get; set; }
        public int GetTotalSubRegionCount { get; set; }
        public int GetTotalCountryCount { get; set; }
        public int GetTotalCityCount { get; set; }
        public int GetTotalCountyCount { get; set; }
        public int GetTotalVisitedCountyCount { get; set; }
    }
}
