using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy
{
    class AlreadyOpenedDBConnectionExeption : Exception
    {
        public AlreadyOpenedDBConnectionExeption()
        {
            //TODO:Dodaj cos?
        }

        public AlreadyOpenedDBConnectionExeption(string message)
            : base(message)
        {
        }

        public AlreadyOpenedDBConnectionExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
