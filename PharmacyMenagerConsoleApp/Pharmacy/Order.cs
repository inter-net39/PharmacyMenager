using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class Order : ActiveRecord
    {
        private readonly int _prescriptionID;
        private readonly int _medicineID;
        private readonly string _date;
        private readonly int _amount;

        public Order(int prescriptionID, int medicineID, string date, int amount)
        {
            _prescriptionID = prescriptionID;
            _medicineID = medicineID;
            _date = date;
            _amount = amount;

            new LogHandler(this);
        }

        public override void Save()
        {
            Open();

            if (ID == 0) //Dodaj nowy rekord.
            {
                SqlTransaction transaction = _connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "INSERT INTO [Orders] (ID, PrescriptionID, MedicineID, Date, Amount)" +
                                  "VALUES('@ID', '@PrescriptionID', '@MedicineID', '@Date', '@Amount'); ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                    Transaction = transaction
                };
               


                SqlParameter parameterID = new SqlParameter()
                {
                    ParameterName = "@ID",
                    Value = 0,
                    DbType = DbType.Int32
                };
                SqlParameter parameterPrescriptionID = new SqlParameter()
                {
                    ParameterName = "@PrescriptionID",
                    Value = _prescriptionID,
                    DbType = DbType.Int32
                };
                SqlParameter parameterMedicineID = new SqlParameter()
                {
                    ParameterName = "@MedicineID",
                    Value = _medicineID,
                    DbType = DbType.Int32
                };
                SqlParameter parameterDate = new SqlParameter()
                {
                    ParameterName = "@Date",
                    Value = _date,
                    DbType = DbType.String
                };
                SqlParameter parameterAmount = new SqlParameter()
                {
                    ParameterName = "@Amount",
                    Value = _amount,
                    DbType = DbType.Int32
                };

                cmd.Parameters.Add(parameterID);
                cmd.Parameters.Add(parameterPrescriptionID);
                cmd.Parameters.Add(parameterMedicineID);
                cmd.Parameters.Add(parameterDate);
                cmd.Parameters.Add(parameterAmount);

                try
                {
                    cmd.ExecuteNonQuery();  

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    Close();
                }
            }
            else
            {
                //TODO 1: Modyfikacja
                //TODO 2: if ID not exist in DB
            }
           
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}