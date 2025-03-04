using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidProductDetailsException : Exception
    {
        public InvalidProductDetailsException() : base(String.Format("Product name cannot be empty! Try again.")) { }

        public InvalidProductDetailsException(string message) : base(message)
        {
        }
    }
}
