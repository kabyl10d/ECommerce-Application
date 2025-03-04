using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAppFinal.Exceptions
{
    internal class PaymentFailedException : Exception
    {
        public PaymentFailedException() : base(String.Format("Payment failed! Order not placed.")) { }
        public PaymentFailedException(string message) : base(message)
        {
        }
    }
}
