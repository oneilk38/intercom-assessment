using System;
using System.Reflection.Metadata;
using System.Text.Json;
using Intercom.Library.CustomerInviter;
using Xunit;

namespace Intercom.Tests.UnitTests
{
    public class CustomerTests
    {
        [Fact]
        public void ParseCoordinates_Should_Return_False_For_Invalid_Lat_Long()
        {
            var valueA = "{ \"latitude\" : \"52.986375\", \"user_id\" : 12, \"name\" : \"Christina McArdle\", \"longitude\" : \"invalid\" }";
            var valueB = "{ \"latitude\" : \"fghgfhgfh\", \"user_id\" : 12, \"name\" : \"Christina McArdle\", \"longitude\" : \"52.986375\" }";
            
            var customerA = JsonSerializer.Deserialize<Customer>(valueA);
            var customerB = JsonSerializer.Deserialize<Customer>(valueB);
            
            Assert.False(customerA.ParseCoordinates());
            Assert.False(customerB.ParseCoordinates());
        }
        
        [Fact]
        public void ParseCoordinates_Should_Return_True_For_Valid_Lat_Long()
        {
            var valueA = "{ \"latitude\" : \"52.986375\", \"user_id\" : 12, \"name\" : \"Christina McArdle\", \"longitude\" : \"52.986375\" }";
            var valueB = "{ \"latitude\" : \"52.986375\", \"user_id\" : 12, \"name\" : \"Christina McArdle\", \"longitude\" : \"52.986375\" }";
            
            var customerA = JsonSerializer.Deserialize<Customer>(valueA);
            var customerB = JsonSerializer.Deserialize<Customer>(valueB);
            
            Assert.True(customerA.ParseCoordinates());
            Assert.True(customerB.ParseCoordinates());
        }

        [Fact]
        public void ToString_Should_Output_In_Expected_Format()
        {
            var valueA = "{ \"latitude\" : \"52.986375\", \"user_id\" : 1, \"name\" : \"Christina McArdle\", \"longitude\" : \"52.986375\" }";
            var valueB = "{ \"latitude\" : \"52.986375\", \"user_id\" : 2, \"name\" : \"Kevin O'Neill\", \"longitude\" : \"52.986375\" }";
            
            var customerA = JsonSerializer.Deserialize<Customer>(valueA);
            var customerB = JsonSerializer.Deserialize<Customer>(valueB);
            
            var expectedOutputA = "{ \"name\" : \"Christina McArdle\", \"user_id\" : 1 }"; 
            var expectedOutputB = "{ \"name\" : \"Kevin O'Neill\", \"user_id\" : 2 }";

            Assert.Equal(customerA.ToString(), expectedOutputA);
            Assert.Equal(customerB.ToString(), expectedOutputB);
        }
    }
}