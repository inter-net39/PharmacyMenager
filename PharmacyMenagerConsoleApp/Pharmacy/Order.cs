using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class Order : ActiveRecord
    {
        private int _prescriptionID;
        private int _medicineID;
        private string _date;
        private int _amount;

        public Order(Prescription prescriptionObj, Medicine medicineObj, string date, int amount)
        {
            _prescriptionID = prescriptionObj.ID;
            _medicineID = medicineObj.ID;
            _date = date;
            _amount = amount;

            new LogHandler(this);
        }

        public override event Action<string> OnSuccesAction;
        public override event Action<string> OnFailAction;

        public override void Save()
        {
            Open();
            if (ID == 0) //Dodaj nowy rekord.
            {
                AddNewRow();
            }
            else
            {
                //TODO 1: Modyfikacja
                //TODO 2: if ID not exist in DB
            }
            //todo: to cos nie dziala.??

        }

        private void AddNewRow()
        {
            if (ID == 0) //Dodaj nowy rekord.
            {
                SqlTransaction transaction = _connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "INSERT INTO [MyPharmacyDB].[dbo].[Orders] ( [PrescriptionID], [MedicineID], [Date], [Amount])" +
                                  "VALUES( @PrescriptionID, @MedicineID, @Date, @Amount);" +
                                  "SELECT SCOPE_IDENTITY(); ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                    Transaction = transaction
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

                cmd.Parameters.Add(parameterPrescriptionID);
                cmd.Parameters.Add(parameterMedicineID);
                cmd.Parameters.Add(parameterDate);
                cmd.Parameters.Add(parameterAmount);

                try
                {
                    cmd.ExecuteNonQuery();
                    ID = Convert.ToInt32(cmd.ExecuteScalar());

                    transaction.Commit();
                    OnSuccesAction?.Invoke($"[Orders] - Pomyślnie dodano rekord. ID = {ID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex.StackTrace);
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
            /*
             * private readonly int _prescriptionID;
               private readonly int _medicineID;
               private readonly string _date;
               private readonly int _amount;
             */
            Open();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "SELECT *" +
                              "FROM [MyPharmacyDB].[dbo].[Orders]" +
                              "WHERE [ID] = @id",
                CommandType = CommandType.Text,
                Connection = _connection,
            };

            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@id",
                Value = ID,
                DbType = DbType.Int32
            };

            cmd.Parameters.Add(para1);

            try
            {

                using (SqlDataReader sqlReader = cmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            if (sqlReader.FieldCount == 5)
                            {
                                ID = Convert.ToInt32(sqlReader.GetValue(0));
                                _prescriptionID = Convert.ToInt32(sqlReader.GetValue(1));
                                _medicineID = Convert.ToInt32(sqlReader.GetValue(2));
                                _date = Convert.ToString(sqlReader.GetValue(3));
                                _amount = Convert.ToInt32(sqlReader.GetValue(4));
                            }
                            else
                            {
                                OnFailAction?.Invoke("ZłyFieldCount.");
                            }
                        }
                    }
                    else
                    {
                        OnFailAction?.Invoke($"Klient o identyfikatorze {ID} nie istnieje.");
                        throw new Exception("ID = 0, lub rekord nie istnieje.");
                    }
                    OnSuccesAction?.Invoke($"[Prescriptions] - Pomyślnie odświerzono rekord. ID = {ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                OnFailAction?.Invoke($"{ex.Message}");
            }
            finally
            {
                Close();

            }
        }


        public override void Remove()
        {
            Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DeleteOrder";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _connection;

            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@id",
                Value = ID,
                DbType = DbType.Int32,
                Direction = ParameterDirection.Input,
            };

            cmd.Parameters.Add(para1);

            try
            {
                cmd.ExecuteNonQuery();
                ID = Convert.ToInt32(cmd.ExecuteScalar());

                OnSuccesAction?.Invoke($"[Orders] - Pomyślnie usunięto rekord. ID = {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
            finally
            {
                Close();
            }
        }
    }
}