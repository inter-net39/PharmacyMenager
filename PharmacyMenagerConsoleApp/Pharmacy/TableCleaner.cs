using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class TableCleaner : ActiveRecord
    {
        public override event Action<string> OnSuccesAction;
        public override event Action<string> OnFailAction;

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

        public void CleanAllDataInTables()
        {
            Open();

            SqlTransaction transaction = _connection.BeginTransaction();

            SqlCommand cmd = new SqlCommand()
            {
                CommandText = "DELETE FROM [MyPharmacyDB].[dbo].[Medicines]",
                              
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            }; SqlCommand cmd2 = new SqlCommand()
            {
                CommandText = "DELETE FROM [MyPharmacyDB].[dbo].[Orders]",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            }; SqlCommand cmd3 = new SqlCommand()
            {
                CommandText = "DELETE FROM [MyPharmacyDB].[dbo].[Prescriptions]",

                Connection = _connection,
                Transaction = transaction
            };

            try
            {
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                transaction.Commit();
                OnSuccesAction?.Invoke($"[TableCleaner] - Pomyślnie usunięto dane z [Medicines].");
                OnSuccesAction?.Invoke($"[TableCleaner] - Pomyślnie usunięto dane z [Orders].");
                OnSuccesAction?.Invoke($"[TableCleaner] - Pomyślnie usunięto dane z [Prescriptions].");

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
    }
}