using System;
using Intercom.Library.CustomerInviter.Interfaces;

namespace Intercom.Library.CustomerInviter
{
    public class DistanceCalculator
    {
        public static double RadiusOfEarth = 6372.8; 
        
        public static double DegreesToRadians = Math.PI / 180D;

        public static double CalculateDistance(double latA, double longA, double latB, double longB)
        {
            var deltaLat = (latB - latA) * DegreesToRadians;
            var deltaLong = (longB - longA) * DegreesToRadians;

            var a = Math.Pow(
                        Math.Sin(deltaLat / 2D), 2D) +
                    Math.Cos(latA * DegreesToRadians) *
                    Math.Cos(latB * DegreesToRadians) *
                    Math.Pow(Math.Sin(deltaLong / 2D), 2D);

            var c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            
            return Math.Round(RadiusOfEarth * c, 0); // round to nearest kilometer
        }
        
    }
}