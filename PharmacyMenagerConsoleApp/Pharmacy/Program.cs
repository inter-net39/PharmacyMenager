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

        public static Prescription lastPrescription = null;
        public static Medicine lastMedicine = null;
        public static Order lastOrder = null;

        private static void Main(string[] args)
        {
            try
            {
                DeleteOldLogs();
                //DeleteAllOldDataInDB();
            
                string command = "";
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Dostępne komendy:");
                    Console.WriteLine(@"Metody typu add przy podaniu ID = 0 dodają nowy rekord. W przypadku podania ID > 0 dane podane w poleceniu add modyfikują danu rekord o podanym ID.");
                    Console.WriteLine(@"AddMedicine     [int id],[string name],[string manufacturer],[decimal price],[int amount],[bool withPrescription]");
                    Console.WriteLine(@"AddPrescription [int id],[string customerName],[string pesel],[int prescriptionNumber]");
                    Console.WriteLine(@"AddOrder        [int id],[Prescription prescriptionID],[Medicine medicineID],[string date],[int amount]");       
                    Console.ForegroundColor = ConsoleColor.White;

                    command = Console.ReadLine();
                    string[] commandSplited = command.Split(' ');

                    if (commandSplited.Length == 2)
                    {
                        string commandType = commandSplited[0];
                        string[] commandValues = commandSplited[1].Split(',');
                        NewMethod(commandType, commandValues, lastPrescription, lastMedicine);
                    }
                    else if (commandSplited[0] == "Select")
                    {
                        
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

        private static void NewMethod(string commandType, string[] commandValues, Prescription lastPrescription, Medicine lastMedicine)
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
                        Program.lastMedicine = myMedicine;
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
                        Program.lastPrescription = myPrescription;
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
                        Program.lastPrescription = new Prescription(lastPresciptionNumber);
                    }

                    if (lastMedicineNumber != 0)
                    {
                        Program.lastMedicine = new Medicine(lastMedicineNumber);
                    }
                    string date = Convert.ToString(commandValues[3]);
                    int amount = Convert.ToInt32(commandValues[4]);


                    if (id != null && lastPrescription != null && lastMedicine != null && date != null && amount != null)
                    {
                        Order myOrder = new Order(id, lastPrescription, lastMedicine, date, amount);
                        myOrder.Save();
                        //Program.lastOrder = myOrder;
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
