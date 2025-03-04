using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class NotEnoughStockException : Exception
    {
        public NotEnoughStockException() : base(String.Format("Not enough stock!")) { }

        public NotEnoughStockException(string message) : base(message)
        {
        }
    }
}
