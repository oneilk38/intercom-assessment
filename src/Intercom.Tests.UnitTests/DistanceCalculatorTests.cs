using System.Text.Json;
using Intercom.Library.CustomerInviter;
using Xunit;

namespace Intercom.Tests.UnitTests
{
    public class DistanceCalculatorTests
    {
        [Fact]
        public void Should_Correctly_Generate_Distance()
        {
            // verified manually first using an online distance calculator -> https://www.vcalc.com/wiki/vCalc/Haversine+-+Distance
            var latFrom = 53.339428;
            var longFrom = -6.257664;
            
            Assert.Equal(42, DistanceCalculator.CalculateDistance(latFrom, longFrom, 52.986375, -6.043701));
            Assert.Equal(313, DistanceCalculator.CalculateDistance(latFrom, longFrom, 51.92893, -10.27699));
            Assert.Equal(302, DistanceCalculator.CalculateDistance(latFrom, longFrom, 52.3191841, -10.4240951));
            Assert.Equal(222, DistanceCalculator.CalculateDistance(latFrom, longFrom, 51.8856167, -8.5072391));
            Assert.Equal(109, DistanceCalculator.CalculateDistance(latFrom, longFrom, 53.807778, -7.714444));
        }
    }
}