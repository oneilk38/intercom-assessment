using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Intercom.Library.CustomerInviter;
using Intercom.Library.CustomerInviter.Interfaces;
using Xunit; 
using Moq; 

namespace Intercom.Tests.UnitTests
{
    public class InviterTests
    {
        private readonly Mock<IRadius> radiusMock = new Mock<IRadius>(MockBehavior.Strict); 
        private readonly Mock<IO> ioMock = new Mock<IO>(MockBehavior.Strict);
        private readonly string expectedFilepath = "/usrs/files/customers.txt";
        private readonly string outputFilepath = "/usrs/files/output.txt";
        private readonly List<Customer> inputCustomers = new Customer[4]
        {
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"50.986375\", \"user_id\" : 12, \"name\" : \"Christina McArdle\", \"longitude\" : \"51.986375\" }"),
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"52.986375\", \"user_id\" : 2, \"name\" : \"Kevin ONeill\", \"longitude\" : \"62.986375\" }"),
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"52.986375\", \"user_id\" : 3, \"name\" : \"James Kelly\", \"longitude\" : \"62.986375\" }"),
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"100.986375\", \"user_id\" : 4, \"name\" : \"Steven Foley\", \"longitude\" : \"62.986375\" }")
        }.ToList();

        public void IOMockReadFromFileSetup()
        {
            ioMock
                .Setup(x => x.ReadFromFile(It.Is<string>(inputFilepath => inputFilepath == expectedFilepath)))
                .Returns(inputCustomers);
        }
        
        public void IOMockCreateOutputFileSetup(bool created)
        {
            ioMock
                .Setup(x => x.CreateOutputFile(It.Is<string>(x => x == outputFilepath), It.Is<List<string>>(x => x.Count > 0)))
                .Returns(created); 
        }

        public void RadiusMockIsWithinRadiusSetup(bool isWithinRadius)
        {
            radiusMock
                .Setup(x => x.IsWithinRadius(It.Is<double>(lat => true), It.Is<double>(lng => true)))
                .Returns(isWithinRadius); 
        }

        // here we mock the interface that reads from file, we want to test that the constructor will assign whatever is returned from IO to customers property, we are not testing that actual integration yet 
        [Fact]
        public void Constructor_Should_Assign_Properties_And_Read_Customers_From_IO()
        {
            IOMockReadFromFileSetup();

            var customerInviter = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object); 
            
            Assert.Equal(customerInviter.GetCustomers(), inputCustomers);
        }
        
        // again we do not care what IsWithinRadius actually does at this point, we just want to check that our function will correctly filter depending on what IsWithinRadius returns 
        [Fact]
        public void GetCustomersWithinRadius_Should_Filter_Out_Customers_Based_On_Result_Of_IsWithinRadius_Call()
        {
            IOMockReadFromFileSetup();
            
            RadiusMockIsWithinRadiusSetup(true);

            var customerInviterWithTrueRadiusMock = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object); 
            
            // always return true so entire list should be returned 
            Assert.Equal(customerInviterWithTrueRadiusMock.GetCustomersWithinRadius(), inputCustomers);
            
            RadiusMockIsWithinRadiusSetup(false);
            
            var customerInviterWithFalseRadiusMock = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object);
            // always return false so nothing should be returned 
            Assert.Equal(customerInviterWithFalseRadiusMock.GetCustomersWithinRadius(), new List<Customer>());
        }

        [Fact]
        public void InviteCustomers_Should_Return_False_If_No_Customers_Within_Radius()
        {
            IOMockReadFromFileSetup();
            
            RadiusMockIsWithinRadiusSetup(false);

            var customerInviterWithFalseRadiusMock = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object);
            
            Assert.False(customerInviterWithFalseRadiusMock.InviteCustomers(outputFilepath));
        }
        
        [Fact]
        public void InviteCustomers_Should_Return_False_If_Customers_Within_Radius_But_Failed_To_Write_Output()
        {
            IOMockReadFromFileSetup();
            
            RadiusMockIsWithinRadiusSetup(true);
            IOMockCreateOutputFileSetup(false);
            
            var customerInviterWithFalseRadiusMock = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object);
            
            IOMockCreateOutputFileSetup(false);
            Assert.False(customerInviterWithFalseRadiusMock.InviteCustomers(outputFilepath));
        }
        
        [Fact]
        public void InviteCustomers_Should_Return_True_If_Customers_Within_Radius_And_Wrote_Output()
        {
            IOMockReadFromFileSetup();
            
            RadiusMockIsWithinRadiusSetup(true);
            IOMockCreateOutputFileSetup(true);
            
            var customerInviterWithFalseRadiusMock = new Inviter(ioMock.Object, expectedFilepath, radiusMock.Object);
            
            IOMockCreateOutputFileSetup(true);
            Assert.True(customerInviterWithFalseRadiusMock.InviteCustomers(outputFilepath));
        }
    }
}