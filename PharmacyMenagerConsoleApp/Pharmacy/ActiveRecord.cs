using System;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy
{
    public abstract class ActiveRecord
    {
        protected SqlConnection _connection;
        public event Action<string> OnCloseAction;
        public event Action<string> OnCloseActionERR;
        public abstract event Action<string> OnSuccesAction;
        public abstract event Action<string> OnFailAction;

        public int ID { get; protected set; }
        /// <summary>
        /// Metoda Save w klasach dziedziczących po klasie
        /// ActiveRecord musi zapisywać dane obiektu, na
        /// którym została wywołana, do bazy danych.   
        /// 
        /// Jeśli właściwość ID jest ustawiona na 0,
        /// wywołanie metody musi utworzyć encję w bazie,
        /// a po utworzeniu uzupełnić wartość właściwości
        /// ID.
        /// 
        /// Jeśli właściwość ID jest ustawiona na wartość
        /// inną niż 0, dane encji zostaną zaktualizowane na
        /// podstawie właściwości.
        /// 
        /// Jeśli encja o takim identyfikatorze nie istnieje
        /// metoda musi rzucić wyjątek.   
        /// </summary>
        public abstract void Save();
        /// <summary>
        /// Metoda Reload w klasach potomnych powinna
        /// zaktualizować dane w obiekcie klasy.
        ///
        /// Jeśli metoda zostanie wywołana na obiekcie,
        /// którego właściwość ID nie istnieje w bazie
        /// danych lub jego wartość wynosi 0, metoda rzuci
        /// wyjątek.
        /// </summary>
        public abstract void Reload();

        protected void Open()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection()
                {
                    ConnectionString = @"Data Source=(local)\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                };
                _connection.Open();
                OnCloseAction?.Invoke("Połączono z bazą danych");
            }
            else
            {
                OnCloseActionERR?.Invoke("AlreadyOpenedDBConnectionExeption: Jesteś już połączony z bazą danych.");
                throw new AlreadyOpenedDBConnectionExeption("Jesteś już połączony z bazą danych.");
            }

        }

        protected void Close()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection = null;
                OnCloseAction?.Invoke("Zamknięto połączenie z bazą danych");
            }
            else
            {
                OnCloseActionERR?.Invoke("NotOpenedDBConnectionException: Baza danych nie jest otwarta.");
                throw new NotOpenedDBConnectionException("Baza danych nie jest otwarta.");
            }
        }

        /// <summary>
        /// Metoda Remove powinna usuwać encję o
        /// podanym identyfikatorze z bazy danych.
        /// Do usunięcia encji z bazy danych użyj
        /// utworzonych procedur składowanych.
        /// Nie zapomnij, że CommandType komendy
        /// powinien być ustawiony na: StoredProcedure
        ///
        /// Jeśli metoda zostanie wywołana na obiekcie,
        /// którego właściwość ID nie istnieje w bazie
        /// danych lub jego wartość wynosi 0, metoda rzuci
        /// wyjątek
        /// </summary>
        public abstract void Remove();

    }
}