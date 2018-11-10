using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class Prescription : ActiveRecord
    {
        public Prescription(int id, string customerName, string pesel, int prescriptionNumber)
        {
            ID = id;
            CustomerName = customerName;
            PESEL = pesel;
            PrescriptionNumber = prescriptionNumber;

            new LogHandler(this);

        }

        public string CustomerName { get; private set; }
        public string PESEL { get; private set; }
        public int PrescriptionNumber { get; private set; }


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
        }
        private void AddNewRow()
        {
            SqlTransaction transaction = _connection.BeginTransaction();

            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "INSERT INTO [MyPharmacyDB].[dbo].[Prescriptions] ( [CustomerName], [PESEL], [PrescriptionNumber])" +
                              "VALUES( @CustomerName, @PESEL, @PrescriptionNumber);" +
                              "SELECT SCOPE_IDENTITY(); ",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };


            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@CustomerName",
                Value = CustomerName,
                DbType = DbType.String
            };
            SqlParameter para2 = new SqlParameter()
            {
                ParameterName = "@PESEL",
                Value = PESEL,
                DbType = DbType.String
            };
            SqlParameter para3 = new SqlParameter()
            {
                ParameterName = "@PrescriptionNumber",
                Value = PrescriptionNumber,
                DbType = DbType.Int32
            };


            cmd.Parameters.Add(para1);
            cmd.Parameters.Add(para2);
            cmd.Parameters.Add(para3);


            try
            {
                cmd.ExecuteNonQuery();
                ID = Convert.ToInt32(cmd.ExecuteScalar());

                transaction.Commit();
                OnSuccesAction?.Invoke($"[Prescriptions] - Pomyślnie dodano rekord. ID = {ID}");

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
            Open();
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "SELECT *" +
                              "FROM [MyPharmacyDB].[dbo].[Prescriptions]" +
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
                            if (sqlReader.FieldCount == 4)
                            {
                                ID = Convert.ToInt32(sqlReader.GetValue(0));
                                CustomerName = Convert.ToString(sqlReader.GetValue(1));
                                PESEL = Convert.ToString(sqlReader.GetValue(2));
                                PrescriptionNumber = Convert.ToInt32(sqlReader.GetValue(3));
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
            cmd.CommandText = "DeletePrescription";
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

                OnSuccesAction?.Invoke($"[Prescriptions] - Pomyślnie usunięto rekord. ID = {ID}");
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