using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy
{
    class NotOpenedDBConnectionException : Exception
    {
        public NotOpenedDBConnectionException()
        {
            //TODO:Dodaj cos?
        }

        public NotOpenedDBConnectionException(string message)
            : base(message)
        {
        }

        public NotOpenedDBConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
