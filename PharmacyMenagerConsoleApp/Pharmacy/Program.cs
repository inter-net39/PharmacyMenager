using System;
using System.IO;

namespace Pharmacy
{
    internal class Program
    {
        public static readonly string LogFileName = "Test.txt";
        public static Prescription LastPrescription;
        public static Medicine LastMedicine;
        public static Order LastOrder;

        public static LogHandler Log = new LogHandler();
        private static void Main(string[] args)
        {
            Raport raport = new Raport();

            try
            {
                DeleteOldLogs();

                //DeleteAllOldDataInDB();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Dostępne komendy:");
                Console.WriteLine(@"Metody typu add przy podaniu ID = 0 dodają nowy rekord. W przypadku podania ID > 0 dane podane w poleceniu Add modyfikują dany rekord o podanym ID.");
                Console.ForegroundColor = ConsoleColor.White;


                string command = "";
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(@"AddMedicine     [int id],[string name],[string manufacturer],[decimal price],[int amount],[bool withPrescription]");
                    Console.WriteLine(@"AddPrescription [int id],[string customerName],[string pesel],[int prescriptionNumber]");
                    Console.WriteLine(@"AddOrder        [int id],[Prescription prescriptionID],[Medicine medicineID],[string date],[int amount]");
                    Console.WriteLine(@"Remove Medicine/Prescription/Order");
                    Console.WriteLine(@"Show Medicines/Prescriptions/Orders/Sales");
                    Console.WriteLine(@"Select [Medicine/Prescription/Order] [int id]");
                    Console.ForegroundColor = ConsoleColor.White;

                    command = Console.ReadLine();

                    string[] commandSplited = command.Split(' ');

                    if (commandSplited.Length == 2)
                    {
                        if (commandSplited[0].Contains("Add"))
                        {
                            string commandType = commandSplited[0];
                            string[] commandValues = commandSplited[1].Split(',');
                            AddCMD(commandType, commandValues, LastPrescription, LastMedicine);
                        }
                        else if (commandSplited[0] == "Show")
                        {
                            ShowCMD(raport, commandSplited);
                        }
                        else if (commandSplited[0] == "Remove")
                        {
                            RemoveCMD(commandSplited);
                        }
                    }
                    else if (commandSplited.Length == 3)
                    {
                        SelectCMD(commandSplited);
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
            Console.ReadLine();
        }

        

        private static void SelectCMD(string[] commandSplited)
        {
            var sID = Convert.ToInt32(commandSplited[2]);
            if (commandSplited[0] == "Select" && sID != 0)
            {
                if (commandSplited[1] == "Medicine")
                {
                    Program.LastMedicine = new Medicine(sID);
                }
                else if (commandSplited[1] == "Prescription")
                {
                    Program.LastPrescription = new Prescription(sID);
                }
                else if (commandSplited[1] == "Order")
                {
                    Program.LastOrder = new Order(sID);
                }
                else
                {
                    Console.WriteLine("Niepoprawna komenda.");
                }
            }
            else
            {
                Console.WriteLine("Niepoprawna komenda.");
            }
        }

        private static void ShowCMD(Raport raport, string[] commandSplited)
        {
            if (commandSplited[1] == "Medicines")
            {
                raport.ShowMedicines();
            }
            else if (commandSplited[1] == "Orders")
            {
                raport.ShowOrders();
            }
            else if (commandSplited[1] == "Prescriptions")
            {
                raport.ShowPrescriptions();
            }
            else if (commandSplited[1] == "Sales")
            {
                raport.Reload();
            }
            else
            {
                Console.WriteLine("Niepoprawna komenda.");
            }
        }

        private static void RemoveCMD(string[] commandSplited)
        {
            if (commandSplited[1] == "Medicine")
            {
                Console.WriteLine("Wprowadz ID do usunięcia, lub wpisz Yes aby usnąć wczesniej wybrany.");
                string com = Console.ReadLine();
                int val = Convert.ToInt32(com);
                if (com.Contains("Yes") && LastMedicine != null)
                {
                    LastMedicine.Remove();
                    LastMedicine = null;
                }
                else if (val > 0)
                {
                    LastMedicine = new Medicine(val);
                    if (LastMedicine != null)
                    {
                        LastMedicine.Remove();
                        LastMedicine = null;
                    }
                    else
                    {
                        Console.WriteLine("Błąd.");
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawna komenda.");
                }
            }
            else if (commandSplited[1] == "Order")
            {
                Console.WriteLine("Wprowadz ID do usunięcia, lub wpisz Yes aby usnąć wczesniej wybrany.");
                string com = Console.ReadLine();
                int val = Convert.ToInt32(com);
                if (com.Contains("Yes") && LastOrder != null)
                {
                    LastOrder.Remove();
                    LastOrder = null;
                }
                else if (val > 0)
                {
                    LastOrder = new Order(val);
                    if (LastOrder != null)
                    {
                        LastOrder.Remove();
                        LastOrder = null;
                    }
                    else
                    {
                        Console.WriteLine("Błąd.");
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawna komenda.");
                }
            }
            else if (commandSplited[1] == "Prescription")
            {
                Console.WriteLine("Wprowadz ID do usunięcia, lub wpisz Yes aby usnąć wczesniej wybrany.");
                string com = Console.ReadLine();
                int val = Convert.ToInt32(com);
                if (com.Contains("Yes") && LastPrescription != null)
                {
                    LastPrescription.Remove();
                    LastPrescription = null;
                }
                else if (val > 0)
                {
                    LastPrescription = new Prescription(val);
                    if (LastPrescription != null)
                    {
                        LastPrescription.Remove();
                        LastPrescription = null;
                    }
                    else
                    {
                        Console.WriteLine("Błąd.");
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawna komenda.");
                }
            }
            else
            {
                Console.WriteLine("Niepoprawna komenda.");
            }
        }

        private static void AddCMD(string commandType, string[] commandValues, Prescription lastPrescription, Medicine lastMedicine)
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

                    Medicine myMedicine = new Medicine(id, name, manufacturer, price, amount, withPrescription);
                    myMedicine.Save();
                    Program.LastMedicine = myMedicine;
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

                    Prescription myPrescription = new Prescription(id, customerName, pesel, prescriptionNumber);
                    myPrescription.Save();
                    Program.LastPrescription = myPrescription;
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
                    int lastMedicineNumber = Convert.ToInt32(commandValues[2]);
                    if (lastPresciptionNumber != 0)
                    {
                        Program.LastPrescription = new Prescription(lastPresciptionNumber);
                    }
                    if (lastMedicineNumber != 0)
                    {
                        Program.LastMedicine = new Medicine(lastMedicineNumber);
                    }
                    string date = Convert.ToString(commandValues[3]);
                    int amount = Convert.ToInt32(commandValues[4]);
                    if (LastPrescription != null && LastMedicine != null)
                    {
                        Order myOrder = new Order(id, LastPrescription, LastMedicine, date, amount);
                        myOrder.Save();
                        Program.LastOrder = myOrder;
                    }
                    else
                    {
                        Console.WriteLine("Nie udało się utworzyć [Order]");
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
            if (File.Exists(LogFileName))
            {
                File.Delete(LogFileName);
            }
        }
    }
}
