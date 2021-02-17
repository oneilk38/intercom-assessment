using System.Collections.Generic;

namespace Intercom.Library.CustomerInviter.Interfaces
{
    public interface IO
    {
        public List<Customer> ReadFromFile(string inputFilename);
        public bool CreateOutputFile(string outputFilename, List<string> output);
    }
}