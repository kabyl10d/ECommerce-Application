using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base(String.Format("Username cannot be empty! Try again.")) { }
        public InvalidUsernameException(string message) : base(message)
        {
        }
    }
}
