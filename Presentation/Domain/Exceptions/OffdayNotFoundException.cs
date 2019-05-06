using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class OffdayNotFoundException : NotFoundException
    {
        public OffdayNotFoundException()
        {
        }

        public OffdayNotFoundException(string message) : base(message)
        {
        }
    }
}
