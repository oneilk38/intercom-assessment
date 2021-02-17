using Intercom.Library.CustomerInviter;
using Xunit;

namespace Intercom.Tests.UnitTests
{
    public class RadiusTests
    {
        // We have verified distance in previous tests, for true, put radius higher than distances, for false put radius lower than distances 
        [Fact]
        public void IsWithinRadius_Should_Return_True()
        {
            var radius = new Radius(53.339428, -6.257664, 150); 
            
            
            Assert.True(radius.IsWithinRadius(52.986375, -6.043701));
            Assert.True(radius.IsWithinRadius(53.807778, -7.714444));
        }
        
        [Fact]
        public void IsWithinRadius_Should_Return_False()
        {
            var radius = new Radius(53.339428, -6.257664, 150); 
            
            Assert.False(radius.IsWithinRadius(51.92893, -10.27699));
            Assert.False(radius.IsWithinRadius(53.807778, -10.4240951));
            Assert.False(radius.IsWithinRadius(51.8856167, -8.5072391));
        }
    }
}