using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataClasses
{
    public class DataAssignment : Assignment
    {
        public DataAssignment(Workform workform, int duration) : base(workform, duration)
        {
        }
    }
}
