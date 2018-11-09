using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Dodatkowo potrzebujesz metod pobierających
            //TODO: dane z bazy danych.
            //TODO: Metody muszą zwracać uzupełnione obiekty na
            //TODO: podstawie danych pobranych z bazy.

            //TODO KOŃCOWE: Klasa raportu.
        }
    }

    public abstract class ActiveRecord
    {
        public int ID { get; }
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
            //TODO :OPEN DB CONNECTION
        }

        protected void Close()
        {
            //TODO :CLOSE DB CONNECTION
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
    public class Medicine
    {
   
    }
    public class Order
    {

    }
    public class Prescription
    {

    }
}
