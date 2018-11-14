using System;
using System.IO;

namespace Pharmacy
{
    public class LogHandler
    {
        public void AddLogMaker(Object sender)
        {
            try
            {
                if (sender is Order)
                {
                    (sender as Order).OnCloseAction += AddLogSucces;
                    (sender as Order).OnCloseActionERR += AddLoggFail;
                    (sender as Order).OnFailAction += AddLoggFail;
                    (sender as Order).OnSuccesAction += AddLogSucces;
                }
                else if (sender is Medicine)
                {
                    (sender as Medicine).OnCloseAction += AddLogSucces;
                    (sender as Medicine).OnCloseActionERR += AddLoggFail;
                    (sender as Medicine).OnFailAction += AddLoggFail;
                    (sender as Medicine).OnSuccesAction += AddLogSucces;
                }
                else if (sender is Prescription)
                {
                    (sender as Prescription).OnCloseAction += AddLogSucces;
                    (sender as Prescription).OnCloseActionERR += AddLoggFail;
                    (sender as Prescription).OnFailAction += AddLoggFail;
                    (sender as Prescription).OnSuccesAction += AddLogSucces;
                }
                else if (sender is Raport)
                {
                    (sender as Raport).OnCloseAction += AddLogSucces;
                    (sender as Raport).OnCloseActionERR += AddLoggFail;
                    (sender as Raport).OnFailAction += AddLoggFail;
                    (sender as Raport).OnSuccesAction += AddLogSucces;
                }
                else if (sender is TableCleaner)
                {
                    (sender as TableCleaner).OnCloseAction += AddLogSucces;
                    (sender as TableCleaner).OnCloseActionERR += AddLoggFail;
                    (sender as TableCleaner).OnFailAction += AddLoggFail;
                    (sender as TableCleaner).OnSuccesAction += AddLogSucces;
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
                using (StreamWriter sw = new StreamWriter(Program.LogFileName, true))
                {
                    sw.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Fail: {message}");
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Fail: {message}");
                Console.ForegroundColor = ConsoleColor.White;
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
                using (StreamWriter sw = new StreamWriter(Program.LogFileName, true))
                {
                    sw.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Succes: {message}");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Succes: {message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e.StackTrace);
            }

        }

    }
}
