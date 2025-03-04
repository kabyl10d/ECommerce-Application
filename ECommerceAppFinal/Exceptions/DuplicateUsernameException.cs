using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException() : base(String.Format("Username already exists!")) { }
        public DuplicateUsernameException(string message) : base(message)
        {
        }
    }
}
