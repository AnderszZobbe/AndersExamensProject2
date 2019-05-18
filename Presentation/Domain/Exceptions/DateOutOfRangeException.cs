using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class DateOutOfRangeException : ArgumentOutOfRangeException
    {
        public DateOutOfRangeException()
        {
        }

        public DateOutOfRangeException(string message) : base(message)
        {
        }
    }
}
