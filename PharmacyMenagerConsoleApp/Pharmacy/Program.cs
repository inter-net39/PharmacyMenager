using System;
using System.Diagnostics;
using System.IO;

namespace Pharmacy
{
    internal class Program
    {
        public static readonly string FILENAME = "Test.txt";

        private static void Main(string[] args)
        {
            try
            {
                DeleteOldLogs();
                //TODO: Dodatkowo potrzebujesz metod pobierających
                //TODO: dane z bazy danych.
                //TODO: Metody muszą zwracać uzupełnione obiekty na
                //TODO: podstawie danych pobranych z bazy.
                //TODO KOŃCOWE: Klasa raportu.


                Order order = new Order(0,1, "2019-11-27",5);
                order.Save();

                Process.Start("notepad.exe", $".\\{FILENAME}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.ReadLine();
        }

        private static void DeleteOldLogs()
        {
            if (File.Exists(FILENAME))
            {
                File.Delete(FILENAME);
            }
        }
    }
}
