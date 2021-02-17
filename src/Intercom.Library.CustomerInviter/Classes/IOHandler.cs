using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Intercom.Library.CustomerInviter.Interfaces;

namespace Intercom.Library.CustomerInviter
{
    public class IOHandler : IO 
    {

        public List<Customer> ReadFromFile(string inputFilename)
        {
            var customers = new List<Customer>(); 
            
            var input = System.IO.File.ReadAllLines(inputFilename);
            
            foreach (string line in input)
            {
                try
                {
                    Customer c = JsonSerializer.Deserialize<Customer>(line);
                    if (c.ParseCoordinates())
                        customers.Add(c);
                    else 
                        Console.WriteLine("Invalid coordinates"); 
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Invalid JSON {line}, exception: {e}");
                }
            }

            return customers; 
        }

        public bool CreateOutputFile(string outputFilename, List<string> output)
        {
            if (output.Count == 0)
            {
                Console.WriteLine("No customers to invite.");
                return false;
            }


            if (File.Exists(outputFilename))
            {
                Console.WriteLine("Invitations already sent, not overwriting output file.");
                return false;
            }

            try
            {
                File.WriteAllLinesAsync(outputFilename, output);
                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine($"IO Exception occured, failed to invite customers. Exception : {e}");
                return false;
            }
        }
    }
}