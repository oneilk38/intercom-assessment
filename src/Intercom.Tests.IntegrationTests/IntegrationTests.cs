using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Intercom.Library.CustomerInviter;
using Intercom.Library.CustomerInviter.Interfaces;
using Intercom.Service.CustomerInviter;
using Moq;
using Xunit;

namespace Intercom.Tests.IntegrationTests
{
    // for these tests I will be testing everything except writing & reading from IO
    // the test will be to check that it returns correct boolean AND if written to IO, we call the mocked createOutPutFile function with correct parameters (list of customers)
    public class IntegrationTests 
    {
        private readonly Mock<IO> ioMock = new Mock<IO>(MockBehavior.Strict);
        private readonly Input input = new Input("/files/customers.txt", "/files/output.txt", 53.339428, -6.257664, 100);
        
        private readonly List<Customer> inputCustomersWithinRadius = new Customer[4]
        {
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"54.1225\", \"user_id\" : 27, \"name\" : \"Enid Gallagher\", \"longitude\" : \"-8.143333\" }"), // not within 
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"53.1229599\", \"user_id\" : 6, \"name\" : \"Theresa Enright\", \"longitude\" : \"-6.2705202\" }"), // within  
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"52.2559432\", \"user_id\" : 9, \"name\" : \"Jack Dempsey\", \"longitude\" : \"-7.1048927\" }"), // not within 
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"53.2451022\", \"user_id\" : 4, \"name\" : \"Ian Kehoe\", \"longitude\" : \"-6.238335\" }") // within 
        }.ToList();
        
        private readonly List<Customer> inputCustomersNotWithinRadius = new Customer[2]
        {
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"54.1225\", \"user_id\" : 27, \"name\" : \"Enid Gallagher\", \"longitude\" : \"-8.143333\" }"), // not within 
            JsonSerializer.Deserialize<Customer>("{ \"latitude\" : \"52.2559432\", \"user_id\" : 9, \"name\" : \"Jack Dempsey\", \"longitude\" : \"-7.1048927\" }"), // not within 
        }.ToList();

        public IntegrationTests()
        {
            // this is done in file handler class, as we are mocking, need to manually do it ourselves to ensure we correctly parse inputted lat long strings to double 
            foreach (Customer c in inputCustomersWithinRadius)
                c.ParseCoordinates(); 
            
            foreach (Customer c in inputCustomersNotWithinRadius)
                c.ParseCoordinates(); 
        }
        
        // Set Up 
        public void IOMockReadFromFileSetup(bool someWithin)
        {
            var customers = someWithin ? inputCustomersWithinRadius : inputCustomersNotWithinRadius; 
            ioMock
                .Setup(x => x.ReadFromFile(It.Is<string>(inputFilepath => inputFilepath == input.InputFilePath)))
                .Returns(customers);
        }
        
        public void IOMockCreateOutputFileSetup(bool created, List<string> expectedOutput)
        {
            ioMock
                .Setup(x => x.CreateOutputFile(It.Is<string>(outputFilePath => outputFilePath == input.OutputFilePath), It.Is<List<string>>(output => output.SequenceEqual(expectedOutput) )))
                .Returns(created); 
        }
        
        // Tests 
        
        [Fact]
        public void Should_Write_Customer_Info_To_Output_If_Customers_Within_Radius()
        {
            // read input with customers within radius 
            IOMockReadFromFileSetup(true);

            var expectedOutput = new string[2]
            {
                "{ \"name\" : \"Ian Kehoe\", \"user_id\" : 4 }",
                "{ \"name\" : \"Theresa Enright\", \"user_id\" : 6 }" 
            }.ToList(); 
            
            IOMockCreateOutputFileSetup(true, expectedOutput);
            
            
            
            var result = Program.InviteCustomer(ioMock.Object, new Radius(input.Latitude, input.Longitude, input.RadiusSize), input);
            
            // should return true as customers invited  
            Assert.True(result);
        }
        
        [Fact]
        public void Should_Return_False_If_No_Customer_Within_Radius()
        {
            // read input with no customers within radius 
            IOMockReadFromFileSetup(false);
            
            var result = Program.InviteCustomer(ioMock.Object, new Radius(input.Latitude, input.Longitude, input.RadiusSize), input);
            
            // should return false as no one invited 
            Assert.False(result);
        }
        
        [Fact]
        public void Should_Return_False_If_Customer_Within_Radius_But_Output_File_Already_Exists()
        {
            // read input with customers within radius 
            IOMockReadFromFileSetup(true);

            var expectedOutput = new string[2]
            {
                "{ \"name\" : \"Ian Kehoe\", \"user_id\" : 4 }",
                "{ \"name\" : \"Theresa Enright\", \"user_id\" : 6 }" 
            }.ToList(); 
            
            IOMockCreateOutputFileSetup(false, expectedOutput);

            var result = Program.InviteCustomer(ioMock.Object, new Radius(input.Latitude, input.Longitude, input.RadiusSize), input);
            
            // should return false as output file already exists   
            Assert.False(result);
        }
    }
}