namespace VisitorBook.UI.ViewModels
{
    public class VisitorStatisticViewModel
    {
        public Tuple<string, string> GetHighestCountOfVisitedCityByVisitor { get; set; }
        public Tuple<string, string> GetHighestCountOfVisitedCountyByVisitor { get; set; }
        public Tuple<string, string> GetLongestDistanceByVisitorOneTime { get; set; }
        public Tuple<string, string> GetLongestDistanceByVisitorAllTime { get; set; }
    }
}
