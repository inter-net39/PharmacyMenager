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
            new LogHandler(this);
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
    }
}
