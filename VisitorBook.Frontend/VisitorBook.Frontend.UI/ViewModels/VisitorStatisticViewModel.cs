using VisitorBook.Frontend.UI.Models;

namespace VisitorBook.Frontend.UI.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public HighestCountOfVisitedCityByVisitor GetHighestCountOfVisitedCityByVisitor { get; set; }
        public HighestCountOfVisitedCountyByVisitor GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public LongestDistanceByVisitorOneTime GetLongestDistanceByVisitorOneTime { get; set; }
        public LongestDistanceByVisitorAllTime GetLongestDistanceByVisitorAllTime { get; set; }
    }
}
