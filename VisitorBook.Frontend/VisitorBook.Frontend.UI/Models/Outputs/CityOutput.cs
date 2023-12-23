namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class CityOutput
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public CountryOutput Country { get; set; }
    }
}
