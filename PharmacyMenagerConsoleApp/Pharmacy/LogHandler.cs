using System;
using System.IO;

namespace Pharmacy
{
    public class LogHandler
    {
      

        public LogHandler(object sender)
        {
            try
            {
                if (sender is Order)
                {
                    (sender as Order).OnCloseAction += AddLogSucces;
                    (sender as Order).OnCloseActionERR += AddLoggFail;
                }
                else if (sender is Medicine)
                {
                    (sender as Medicine).OnCloseAction += AddLogSucces;
                    (sender as Medicine).OnCloseActionERR += AddLoggFail;
                }
                else if (sender is Prescription)
                {
                    (sender as Prescription).OnCloseAction += AddLogSucces;
                    (sender as Prescription).OnCloseActionERR += AddLoggFail;
                }
                else
                {
                    throw new Exception("Invalid Object Type.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e.StackTrace);
            }
        }


        private void AddLoggFail(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Program.FILENAME, true))
                {
                    sw.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Fail: {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e.StackTrace);
            }

        }
        private void AddLogSucces(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Program.FILENAME, true))
                {
                    sw.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Succes: {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e.StackTrace);
            }

        }

    }
}
