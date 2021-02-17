using Intercom.Library.CustomerInviter.Interfaces;

namespace Intercom.Library.CustomerInviter
{
    public class Radius : IRadius
    {
        public double Latitude;
        public double Longitude;
        public double RadiusSize;

        public Radius(double latitude, double longitude, double radiusSize)
        {
            Latitude = latitude;
            Longitude = longitude;
            RadiusSize = radiusSize;
        }

        public bool IsWithinRadius(double latitude, double longitude)
        {
            var customerDistanceFromOffice = DistanceCalculator.CalculateDistance(Latitude, Longitude, latitude, longitude);

            return customerDistanceFromOffice <= RadiusSize; 
        }
    }
}