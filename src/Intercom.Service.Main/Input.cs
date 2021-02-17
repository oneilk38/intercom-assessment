using System;
using System.IO;

namespace Intercom.Service.CustomerInviter
{
    public class Input
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusSize { get; set; }

        public Input()
        {
            InputFilePath = GetInputFilepath();
            OutputFilePath = GetOutputFilepath();
            Latitude = GetLatitude();
            Longitude = GetLongitude();
            RadiusSize = GetRadiusSize(); 
        }

        public Input(string inputFilePath, string outputFilePath, double latitude, double longitude, double radiusSize)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            Latitude = latitude;
            Longitude = longitude;
            RadiusSize = radiusSize;
        }
        
        public string GetInputFilepath()
        {
            Console.WriteLine("Enter absolute path to input file : ");
            string inputFilePath;

            while (true)
            {
                inputFilePath = Console.ReadLine();
                if (File.Exists(inputFilePath))
                    break;
                
                Console.WriteLine("No file exists at that location, please try again : ");
            }
            
            Console.WriteLine();

            return inputFilePath;
        }
        
        public string GetOutputFilepath()
        {
            Console.WriteLine("Enter desired absolute path for output file : "); 
            
            string outputFilePath;
            while (true)
            {
                outputFilePath = Console.ReadLine();
                if (File.Exists(outputFilePath))
                    Console.WriteLine("A file already exists at that location, enter a different location : ");
                else
                    break;
            }
            
            Console.WriteLine();

            return outputFilePath;
        }

        public double GetLatitude()
        {
            Console.WriteLine("Enter Latitude of Office : ");
            double latitude;

            while (true)
            {
                try
                {
                    latitude = Convert.ToDouble(Console.ReadLine());
                    if (latitude >= -90 && latitude <= 90)
                        break;

                    Console.WriteLine("Invalid - select a value between -90 and 90 : ");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid - select a value between -90 and 90 : "); 
                }
            }
            
            Console.WriteLine();

            return latitude; 
        }

        public double GetLongitude()
        {
            Console.WriteLine("Enter Longitude of Office : ");
            double longitude;

            while (true)
            {
                try
                {
                    longitude = Convert.ToDouble(Console.ReadLine());
                    if (longitude >= -180 && longitude <= 80)
                        break;

                    Console.WriteLine("Invalid - select a value between -180 and 80 : ");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid - select a value between -180 and 80 : "); 
                }
            }
            
            Console.WriteLine();

            return longitude;
        }

        public double GetRadiusSize()
        {
            Console.WriteLine("Enter Radius size : ");
            double radiusSize;

            while (true)
            {
                try
                {
                    radiusSize = Convert.ToDouble(Console.ReadLine());
                    if(radiusSize <= 0)
                        Console.WriteLine("Invalid - radius size must be greater than 0");
                    else
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid - radius size must be greater than 0");
                }
            }
            
            Console.WriteLine();

            return radiusSize; 
        }
    }
}