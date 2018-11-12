using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class Medicine : ActiveRecord
    {
        private string _name;
        private string _manufacturer;
        private decimal _price;
        private int _amount;
        private bool _withPrescription;

        public Medicine(int id, string name, string manufacturer, decimal price, int amount, bool withPrescription)
        {
            ID = id;
            _name = name;
            _manufacturer = manufacturer;
            _price = price;
            _amount = amount;
            _withPrescription = withPrescription;
            new LogHandler(this);
        }
        public Medicine(int id)
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
                CommandText = "UPDATE [MyPharmacyDB].[dbo].[Medicines]" +
                              "SET [Name] = @name," +
                              "    [Manufacturer] = @manufacturer," +
                              "    [Price] = @price," +
                              "    [Amount] = @amount, " +
                              "    [WithPrescription] = @withPrescription " +
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
                ParameterName = "@name",
                Value = _name,
                DbType = DbType.String
            };
            SqlParameter para2 = new SqlParameter()
            {
                ParameterName = "@manufacturer",
                Value = _manufacturer,
                DbType = DbType.String
            };
            SqlParameter para3 = new SqlParameter()
            {
                ParameterName = "@price",
                Value = _price,
                DbType = DbType.Decimal
            }; SqlParameter para4 = new SqlParameter()
            {
                ParameterName = "@amount",
                Value = _amount,
                DbType = DbType.Int32
            };
            SqlParameter para5 = new SqlParameter()
            {
                ParameterName = "@withPrescription",
                Value = _withPrescription,
                DbType = DbType.Boolean
            };
            cmd.Parameters.Add(para);
            cmd.Parameters.Add(para1);
            cmd.Parameters.Add(para2);
            cmd.Parameters.Add(para3);
            cmd.Parameters.Add(para4);
            cmd.Parameters.Add(para5);
            try
            {
                cmd.ExecuteNonQuery();
                transaction.Commit();
                OnSuccesAction?.Invoke($"[Medicines] - Pomyślnie zmodyfikowano rekord. ID = {ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                transaction.Rollback();
                OnFailAction?.Invoke($"[Medicines] - Nie udało się zmodyfikować rekordu. ID = {ID}");
            }
            finally
            {
                Close();
            }
        }
        private void AddNewRow()
        {
            SqlTransaction transaction = _connection.BeginTransaction();

            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "INSERT INTO [MyPharmacyDB].[dbo].[Medicines] ( [Name], [Manufacturer], [Price], [Amount], [WithPrescription])" +
                              "VALUES( @Name, @Manufacturer, @Price, @Amount ,@WithPrescription);" +
                              "SELECT SCOPE_IDENTITY(); ",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };
            SqlParameter Name = new SqlParameter()
            {
                ParameterName = "@Name",
                Value = _name,
                DbType = DbType.String
            };
            SqlParameter Manufacturer = new SqlParameter()
            {
                ParameterName = "@Manufacturer",
                Value = _manufacturer,
                DbType = DbType.String
            };
            SqlParameter Price = new SqlParameter()
            {
                ParameterName = "@Price",
                Value = _price,
                DbType = DbType.Decimal
            };
            SqlParameter Amount = new SqlParameter()
            {
                ParameterName = "@Amount",
                Value = _amount,
                DbType = DbType.Int32
            };
            SqlParameter WithPrescription = new SqlParameter()
            {
                ParameterName = "@WithPrescription",
                Value = _withPrescription,
                DbType = DbType.Boolean
            };
            cmd.Parameters.Add(Name);
            cmd.Parameters.Add(Manufacturer);
            cmd.Parameters.Add(Price);
            cmd.Parameters.Add(Amount);
            cmd.Parameters.Add(WithPrescription);
            try
            {
                ID = Convert.ToInt32(cmd.ExecuteScalar());
                transaction.Commit();
                OnSuccesAction?.Invoke($"[Medicines] - Pomyślnie dodano rekord. ID = {ID}");
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
                              "FROM [MyPharmacyDB].[dbo].[Medicines]" +
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
                            if (sqlReader.FieldCount == 6)
                            {
                                ID = Convert.ToInt32(sqlReader.GetValue(0));
                                _name = Convert.ToString(sqlReader.GetValue(1));
                                _manufacturer = Convert.ToString(sqlReader.GetValue(2));
                                _price = Convert.ToDecimal(sqlReader.GetValue(3));
                                _amount = Convert.ToInt32(sqlReader.GetValue(4));
                                _withPrescription = Convert.ToBoolean(sqlReader.GetValue(5));
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
                    OnSuccesAction?.Invoke($"[Medicines] - Pomyślnie odświerzono rekord. ID = {ID}");
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
            cmd.CommandText = "[MyPharmacyDB].[dbo].DeleteMedicine";
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
                OnSuccesAction?.Invoke($"[Medicine] - Pomyślnie usunięto rekord. ID = {ID}");
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
    }
}