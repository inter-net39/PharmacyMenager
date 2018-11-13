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

        public Order(int id, Prescription prescriptionObj, Medicine medicineObj, string date, int amount)
        {
            _prescriptionID = prescriptionObj.ID;
            _medicineID = medicineObj.ID;
            _date = date;
            _amount = amount;
            ID = id;
            new LogHandler(this);
        }

        public Order(int id)
        {
            ID = id;
            new LogHandler(this);
            Reload();
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
            else if (ID > 0)
            {
                UpdateRow();
            }
            else
            {
                OnFailAction?.Invoke($"Numer Id nie może być ujemny.");
            }
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
                    ID = Convert.ToInt32(cmd.ExecuteScalar());
                    transaction.Commit();
                    OnSuccesAction?.Invoke($"[Orders] - Pomyślnie dodano rekord. ID = {ID}");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message, ex.StackTrace);
                    transaction.Rollback();
                }
                finally
                {
                    Close();
                }
            }
            else
            {
                UpdateRow();
            }
        }
        private void UpdateRow()
        {
            SqlTransaction transaction = _connection.BeginTransaction();

            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "UPDATE [MyPharmacyDB].[dbo].[Orders]" +
                              "SET [PrescriptionID] = @PrescriptionID," +
                              "    [MedicineID] = @MedicineID," +
                              "    [Date] = @Date," +
                              "    [Amount] = @Amount " +
                              "WHERE ID = @id",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };

            SqlParameter para = new SqlParameter()
            {
                ParameterName = "@id",
                Value = ID,
                DbType = DbType.Int32
            };
            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@PrescriptionID",
                Value = _prescriptionID,
                DbType = DbType.String
            };
            SqlParameter para2 = new SqlParameter()
            {
                ParameterName = "@MedicineID",
                Value = _medicineID,
                DbType = DbType.String
            };
            SqlParameter para3 = new SqlParameter()
            {
                ParameterName = "@Date",
                Value = _date,
                DbType = DbType.Decimal
            }; SqlParameter para4 = new SqlParameter()
            {
                ParameterName = "@Amount",
                Value = _amount,
                DbType = DbType.Int32
            };
            cmd.Parameters.Add(para);
            cmd.Parameters.Add(para1);
            cmd.Parameters.Add(para2);
            cmd.Parameters.Add(para3);
            cmd.Parameters.Add(para4);
            try
            {
                cmd.ExecuteNonQuery();
                transaction.Commit();
                OnSuccesAction?.Invoke($"[Orders] - Pomyślnie zmodyfikowano rekord. ID = {ID}");

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message, ex.StackTrace);
                transaction.Rollback();
                OnFailAction?.Invoke($"[Orders] - Nie udało się zmodyfikować rekordu. ID = {ID}");
            }
            finally
            {
                Close();
            }
        }

        public override void Reload()
        {
            if (ID == 0)
            {
                throw new Exception("Reload - ID nie moze być 0");
            }
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
                //Console.WriteLine(ex.Message, ex.StackTrace);
                OnFailAction?.Invoke($"{ex.Message}");
            }
            finally
            {
                Close();
            }
        }


        public override void Remove()
        {
            Reload();

            Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "[MyPharmacyDB].[dbo].DeleteOrder";
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
                ID = Convert.ToInt32(cmd.ExecuteScalar());
                OnSuccesAction?.Invoke($"[Orders] - Pomyślnie usunięto rekord. ID = {ID}");
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message, ex.StackTrace);
                OnFailAction?.Invoke($"{ex.Message}");
            }
            finally
            {
                Close();
            }
        }
    }
}