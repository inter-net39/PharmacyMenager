using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Pharmacy
{
    internal class Program
    {
        public static readonly string FILENAME = "Test.txt";
        public static Dictionary<int,Medicine> MedicinesDictionary = new Dictionary<int, Medicine>();
        public static Dictionary<int,Prescription> PrescriptionsDictionary = new Dictionary<int, Prescription>();

        private static void Main(string[] args)
        {
            try
            {
                DeleteOldLogs();
                DeleteAllOldDataInDB();
                //TODO: Dodatkowo potrzebujesz metod pobierających
                //TODO: dane z bazy danych.
                //TODO: Metody muszą zwracać uzupełnione obiekty na
                //TODO: podstawie danych pobranych z bazy.
                //TODO KOŃCOWE: Klasa raportu.



                Medicine medicine = new Medicine(0, "Espumisan", "SlabaFirma", (decimal)33.99, 7, false);
                medicine.Save();

                Prescription prescription = new Prescription(0,"Jan","1111111116",1232414);
                prescription.Save();

                Order order = new Order(prescription, medicine, "11-02-2000", 3);
                order.Save();

                medicine.Remove();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }




            Process.Start("notepad.exe", $".\\{FILENAME}");

            Console.ReadLine();
        }

        private static void DeleteAllOldDataInDB()
        {
            TableCleaner tbCleaner = new TableCleaner();
            tbCleaner.CleanAllDataInTables();
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
