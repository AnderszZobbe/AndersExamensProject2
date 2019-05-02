using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class OverlapException : Exception
    {
        public OverlapException()
        {
        }

        public OverlapException(string message) : base(message)
        {
        }
    }
}
