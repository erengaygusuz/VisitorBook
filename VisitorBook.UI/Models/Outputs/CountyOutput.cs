namespace VisitorBook.UI.Models.Outputs
{
    public class CountyOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public CityOutput City { get; set; }
    }
}
