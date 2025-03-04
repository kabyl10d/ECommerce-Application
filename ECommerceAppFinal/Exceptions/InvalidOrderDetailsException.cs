using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidOrderDetailsException : Exception
    {
        public InvalidOrderDetailsException() : base(String.Format("Invalid Order id ! Try again.")) { }

        public InvalidOrderDetailsException(string message) : base(message)
        {
        }
    }
}
