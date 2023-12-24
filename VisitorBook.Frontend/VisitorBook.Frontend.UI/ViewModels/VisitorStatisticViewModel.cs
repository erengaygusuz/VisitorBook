using VisitorBook.Frontend.UI.Models.Outputs;

namespace VisitorBook.Frontend.UI.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public HighestCountOfVisitedCityByVisitorOutput GetHighestCountOfVisitedCityByVisitor { get; set; }
        public HighestCountOfVisitedCountyByVisitorOutput GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public LongestDistanceByVisitorOneTimeOutput GetLongestDistanceByVisitorOneTime { get; set; }
        public LongestDistanceByVisitorAllTimeOutput GetLongestDistanceByVisitorAllTime { get; set; }
    }
}
