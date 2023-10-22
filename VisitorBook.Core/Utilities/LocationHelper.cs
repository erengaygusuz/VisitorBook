using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace VisitorBook.Core.Utilities
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
