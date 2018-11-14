using System;
using System.Data.SqlClient;

namespace Pharmacy
{
    internal class Raport : ActiveRecord
    {
        public override event Action<string> OnSuccesAction;
        public override event Action<string> OnFailAction;

        public Raport()
        {
            Program.Log.AddLogMaker(this);
        }
        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            Open();
            var cmd = new SqlCommand("SELECT * FROM [MyPharmacyDB].[dbo].AmountOfSoldMedicinesInMonth ", _connection);

            // Loads the query results into the table
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                if (sqlReader.HasRows)
                {
                    OnSuccesAction.Invoke("|---------Nazwa-------+--Ilość---|");


                    while (sqlReader.Read())
                    {
                        OnSuccesAction.Invoke("|"+(sqlReader.GetValue(0).ToString()+"|").PadLeft(22)+ (sqlReader.GetValue(1).ToString()+"|").PadLeft(11));
                    }
                }
                else
                {
                    OnFailAction.Invoke($"[Raport] - Brak wyników do wyświetlenia.");
                }
            }

            Close();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
        public void ShowMedicines()
        {
            Open();
            var cmd = new SqlCommand("SELECT * FROM [MyPharmacyDB].[dbo].[Medicines] ", _connection);

            // Loads the query results into the table
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                if (sqlReader.HasRows)
                {
                    OnSuccesAction.Invoke("|-ID-|-------Name-------|--Manufacturer--|--Price--|-Amount-|-With-Prescription-|");


                    while (sqlReader.Read())
                    {
                        OnSuccesAction.Invoke("|" + (sqlReader.GetValue(0) + "|").PadLeft(5) + (sqlReader.GetValue(1) + "|").PadLeft(19) +
                                              (sqlReader.GetValue(2) + "|").PadLeft(17) + (sqlReader.GetValue(3) + "|").PadLeft(10) +
                                              (sqlReader.GetValue(4) + "|").PadLeft(9) + (sqlReader.GetValue(5) + "|").PadLeft(20)
                                              );
                    }
                }
                else
                {
                    OnFailAction.Invoke($"[Raport] - Brak wyników do wyświetlenia.");
                }
            }

            Close();
        }
        public void ShowOrders()
        {
            Open();
            var cmd = new SqlCommand("SELECT * FROM [MyPharmacyDB].[dbo].[Orders] ", _connection);

            // Loads the query results into the table
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                if (sqlReader.HasRows)
                {
                    OnSuccesAction.Invoke("|-ID-|-PrescriptionID-|-MedicineID-|---Date---|-Amount-|");


                    while (sqlReader.Read())
                    {
                        OnSuccesAction.Invoke("|" + (sqlReader.GetValue(0) + "|").PadLeft(5) + (sqlReader.GetValue(1) + "|").PadLeft(17) +
                                              (sqlReader.GetValue(2) + "|").PadLeft(13) + ((sqlReader.GetValue(3) as DateTime?).Value.ToString("dd-MM-yyyy") + "|").PadLeft(11) +
                                              (sqlReader.GetValue(4) + "|").PadLeft(9)
                        );
                    }
                }
                else
                {
                    OnFailAction.Invoke($"[Raport] - Brak wyników do wyświetlenia.");
                }
            }

            Close();
        }
        public void ShowPrescriptions()
        {
            Open();
            var cmd = new SqlCommand("SELECT * FROM [MyPharmacyDB].[dbo].[Prescriptions] ", _connection);

            // Loads the query results into the table
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                if (sqlReader.HasRows)
                {
                    OnSuccesAction.Invoke("|-ID-|-CustomerName-|---PESEL---|-PrescriptionNumber-|");


                    while (sqlReader.Read())
                    {
                        OnSuccesAction.Invoke("|" + (sqlReader.GetValue(0) + "|").PadLeft(5) + (sqlReader.GetValue(1) + "|").PadLeft(15) +
                                              (sqlReader.GetValue(2) + "|").PadLeft(11) + (sqlReader.GetValue(3) + "|").PadLeft(21)
                        );
                    }
                }
                else
                {
                    OnFailAction.Invoke($"[Raport] - Brak wyników do wyświetlenia.");
                }
            }

            Close();
        }

    }
}
