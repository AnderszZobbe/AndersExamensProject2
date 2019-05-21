using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DuplicateObjectException : Exception
    {
        public DuplicateObjectException()
        {
        }

        public DuplicateObjectException(string message) : base(message)
        {
        }
    }
}
