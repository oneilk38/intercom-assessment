using System;
using System.IO;
using Intercom.Library.CustomerInviter;
using Intercom.Library.CustomerInviter.Interfaces;


namespace Intercom.Service.CustomerInviter
{
    public class Program
    {
        public static bool InviteCustomer(IO handler, IRadius radius, Input input)
        {
            Inviter customerInviter = new Inviter(handler, input.InputFilePath, radius);

            return customerInviter.InviteCustomers(input.OutputFilePath);
        }
        
            
        static void Main(string[] args)
        {
            var input = new Input(); 
            
            IOHandler handler = new IOHandler();
            Radius radius = new Radius(input.Latitude, input.Longitude, input.RadiusSize);

            if (InviteCustomer(handler, radius, input))
                Console.WriteLine($"Successfully invited customers. File -> {input.OutputFilePath}");
            else 
                Console.WriteLine("Failed to invite customers.");

        }
    }
}