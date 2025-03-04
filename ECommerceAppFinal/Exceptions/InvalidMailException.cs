using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidMailException : Exception
    {
        public InvalidMailException() : base(String.Format("Mail address cannot be empty! Try again.\n")) { }
        public InvalidMailException(string message) : base(message)
        {
        }
    }
}
