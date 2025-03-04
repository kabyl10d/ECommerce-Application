using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base(String.Format("Invalid username or password!")) { }

        public InvalidLoginException(string message) : base(message)
        {
        }
    }
}
