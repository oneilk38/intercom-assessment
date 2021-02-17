using System;
using System.Collections.Generic;
using System.Linq;
using Intercom.Library.CustomerInviter.Interfaces;

namespace Intercom.Library.CustomerInviter
{
    public class Inviter
    {
        private List<Customer> Customers { get; }
        
        public List<Customer> GetCustomers() => Customers; 
        
        private IO FileHanlder { get; }
        
        private IRadius Radius { get;  }

        public Inviter(IO fh, string fileName, IRadius radius)
        {
            FileHanlder = fh;
            Customers = FileHanlder.ReadFromFile(fileName);
            Radius = radius; 
        }

        public List<Customer> GetCustomersWithinRadius()
        {
            return Customers
                .Where(customer => Radius.IsWithinRadius(customer.actualLat, customer.actualLong))
                .ToList();
        }
        
        public List<string> ToCustomerOutput(List<Customer> customers) => 
            customers
                .OrderBy(x => x.user_id)
                .Select(x => x.ToString())
                .ToList();
        
        public bool InviteCustomers(string outputFilename)
        {
            var customerWithinRadius = GetCustomersWithinRadius();


            if (customerWithinRadius.Count > 0)
            {
                var output = ToCustomerOutput(customerWithinRadius);
                return FileHanlder.CreateOutputFile(outputFilename, output);
            }
            
            Console.WriteLine("There is no customers within radius of given coordinates.");
            
            return false; 
        }
    }
}