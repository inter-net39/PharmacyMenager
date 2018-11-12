using System;
using System.Collections.Generic;
using System.IO;

namespace Pharmacy
{
    internal class Program
    {
        public static readonly string FILENAME = "Test.txt";
        public static Dictionary<int, Medicine> MedicinesDictionary = new Dictionary<int, Medicine>();
        public static Dictionary<int, Prescription> PrescriptionsDictionary = new Dictionary<int, Prescription>();

        private static void Main(string[] args)
        {
            try
            {
                DeleteOldLogs();
                //DeleteAllOldDataInDB();
                ////TODO: Dodatkowo potrzebujesz metod pobierających
                ////TODO: dane z bazy danych.
                ////TODO: Metody muszą zwracać uzupełnione obiekty na
                ////TODO: podstawie danych pobranych z bazy.
                ////TODO KOŃCOWE: Klasa raportu.

                //Medicine medicine = new Medicine(0, "Lek", "Bayer", 33, 10, false);
                //medicine.Save();

                //Prescription prescription = new Prescription(0, "Pan Stanisław", "99900088812", 9999999);
                //prescription.Save();

                //Order order = new Order(0, prescription, medicine, "10-02-2019", 3);
                //order.Save();

                //Raport raport = new Raport();
                //raport.Reload();
                Prescription lastPrescription = null;
                Medicine lastMedicine = null;
                Order lastOrder = null;

                string command = "";
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Dostępne komendy:");
                    Console.WriteLine(@"AddMedicine     [int id],[string name],[string manufacturer],[decimal price],[int amount],[bool withPrescription]");
                    Console.WriteLine(@"AddPrescription [int id],[string customerName],[string pesel],[int prescriptionNumber]");
                    Console.WriteLine(@"AddOrder        [int id],[Prescription prescriptionObj],[Medicine medicineObj],[string date],[int amount]");
                    Console.WriteLine("Dostępne Aliasy:");
                    Console.WriteLine("-lastP = Ostatnio dodany Prescription");
                    Console.WriteLine("-lastM = Ostatnio dodany Medicine");

                    Console.ForegroundColor = ConsoleColor.White;

                    command = Console.ReadLine();
                    string[] commandSplited = command.Split(' ');

                    if (commandSplited.Length == 2)
                    {
                        string commandType = commandSplited[0];
                        string[] commandValues = commandSplited[1].Split(',');
                        NewMethod(commandType, commandValues, lastPrescription, lastMedicine, lastOrder);
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawna komenda.");
                    }


                } while (command.ToLower() != "exit");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

            //Process.Start("notepad.exe", $".\\{FILENAME}");

            Console.ReadLine();
        }

        private static void NewMethod(string commandType, string[] commandValues, Prescription lastPrescription, Medicine lastMedicine, Order lastOrder)
        {
            if (commandType == "AddMedicine")
            {
                if (commandValues.Length == 6)
                {
                    //[int id],[string name],[string manufacturer],[decimal price],[int amount],[bool withPrescription]
                    int id = Convert.ToInt32(commandValues[0]);
                    string name = Convert.ToString(commandValues[1]);
                    string manufacturer = Convert.ToString(commandValues[2]);
                    decimal price = Convert.ToDecimal(commandValues[3]);
                    int amount = Convert.ToInt32(commandValues[4]);
                    bool withPrescription = Convert.ToBoolean(commandValues[5]);

                    if (id != null && name != null && manufacturer != null && price != null && amount != null && withPrescription != null)
                    {
                        Medicine myMedicine = new Medicine(id, name, manufacturer, price, amount, withPrescription);
                        myMedicine.Save();
                        lastMedicine = myMedicine;
                    }
                    else
                    {
                        Console.WriteLine("Argumenty nie pasują swoim typom");
                    }

                }
                else
                {
                    Console.WriteLine("Nieprawidlowa ilość, lub format atrybutów.");
                }
            }
            else if (commandType == "AddPrescription")
            {
                if (commandValues.Length == 4)
                {
                    //[int id],[string customerName],[string pesel],[int prescriptionNumber]
                    int id = Convert.ToInt32(commandValues[0]);
                    string customerName = Convert.ToString(commandValues[1]);
                    string pesel = Convert.ToString(commandValues[2]);
                    int prescriptionNumber = Convert.ToInt32(commandValues[3]);


                    if (id != null && customerName != null && pesel != null && prescriptionNumber != null)
                    {
                        Prescription myPrescription = new Prescription(id, customerName, pesel, prescriptionNumber);
                        myPrescription.Save();
                        lastPrescription = myPrescription;
                    }
                    else
                    {
                        Console.WriteLine("Argumenty nie pasują swoim typom");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidlowa ilość, lub format atrybutów.");
                }
            }
            else if (commandType == "AddOrder")
            {
                if (commandValues.Length == 5)
                {
                    //[int id],[Prescription prescriptionObj],[Medicine medicineObj],[string date],[int amount]
                    int id = Convert.ToInt32(commandValues[0]);
                    int lastPresciptionNumber = Convert.ToInt32(commandValues[1]);
                    int lastMedicineNumber  = Convert.ToInt32(commandValues[2]);
                    if (lastPresciptionNumber != 0)
                    {
                        lastPrescription = new Prescription(lastPresciptionNumber);
                    }

                    if (lastMedicineNumber != 0)
                    {
                        lastMedicine = new Medicine(lastMedicineNumber);
                    }
                    string date = Convert.ToString(commandValues[3]);
                    int amount = Convert.ToInt32(commandValues[4]);


                    if (id != null && lastPrescription != null && lastMedicine != null && date != null && amount != null)
                    {
                        Order myOrder = new Order(id, lastPrescription, lastMedicine, date, amount);
                        myOrder.Save();
                        lastOrder = myOrder;
                    }
                    else
                    {
                        Console.WriteLine("Argumenty nie pasują swoim typom");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidlowa ilość, lub format atrybutów.");
                }
            }
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
