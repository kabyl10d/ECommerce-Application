using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class DuplicateMailException : Exception
    {
        public DuplicateMailException() : base(String.Format("User already exists with this mail id.")) { }
        public DuplicateMailException(string message) : base(message)
        {
        }
    }
}
