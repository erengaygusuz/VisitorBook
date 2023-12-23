namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class CountryOutput
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public SubRegionOutput SubRegion { get; set; }
    }
}
