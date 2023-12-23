namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class CountyOutput
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public CityOutput City { get; set; }
    }
}
