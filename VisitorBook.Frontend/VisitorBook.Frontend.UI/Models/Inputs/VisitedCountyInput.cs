namespace VisitorBook.Frontend.UI.Models.Inputs
{
    public class VisitedCountyInput
    {
        public Guid VisitorId { get; set; }

        public Guid CityId { get; set; }

        public Guid CountyId { get; set; }

        public DateTime VisitDate { get; set; }
    }
}
