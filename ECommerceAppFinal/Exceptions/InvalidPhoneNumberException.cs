using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException() : base(String.Format("Invalid phone number! Try again.\n")) { }
        public InvalidPhoneNumberException(string message) : base(message)
        {
        }
    }
}
