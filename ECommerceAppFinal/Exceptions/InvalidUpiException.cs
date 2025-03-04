using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidUpiException : Exception
    {
        public InvalidUpiException() : base(String.Format("Invalid UPI ID. Try Again.")) { }

        public InvalidUpiException(string message) : base(message)
        {
        }
    }
}
