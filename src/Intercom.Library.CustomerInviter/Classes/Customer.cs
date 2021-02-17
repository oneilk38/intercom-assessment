using System;

namespace Intercom.Library.CustomerInviter
{
    public class Customer
    {
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int user_id { get; set; }
        public double actualLat { get; set; }
        public double actualLong { get; set; }

        public bool ParseCoordinates()
        {
            try
            {
                actualLat = Convert.ToDouble(latitude);
                actualLong = Convert.ToDouble(longitude);
                return true; 
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override string ToString()
        {
            return "{ \"name\" : \"" + name + "\", \"user_id\" : " + user_id + " }" ;
        }

    }
}