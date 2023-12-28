using System.Device.Location;

namespace VisitorBook.Backend.Core.Utilities
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationHelper
    {
        public double GetDistance(Location from, Location to)
        {
            var fromCoordinate = new GeoCoordinate(from.Latitude, from.Longitude);
            var toCoordinate = new GeoCoordinate(to.Latitude, to.Longitude);

            return fromCoordinate.GetDistanceTo(toCoordinate);
        }
    }
}
