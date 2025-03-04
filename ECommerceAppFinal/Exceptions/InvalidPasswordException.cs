using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base(String.Format("Password cannot be empty! Try again.\n")) { }
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
