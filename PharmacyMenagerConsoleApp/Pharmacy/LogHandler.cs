using System;
using System.IO;

namespace Pharmacy
{
    internal class LogManager
    {
        private readonly string _filename;

        public LogManager(string fileName)
        {
            _filename = fileName;
            try
            {
                //Delete file if exists.
                if (File.Exists(_filename)) File.Delete(_filename);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
