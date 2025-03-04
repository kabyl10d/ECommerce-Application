using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidRoleException : Exception
    {
        public InvalidRoleException() : base(String.Format("Invalid role! Try again.")) { }

        public InvalidRoleException(string message) : base(message)
        {
        }
    }
}
