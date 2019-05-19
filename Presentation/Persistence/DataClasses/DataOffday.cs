using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataClasses
{
    public class DataOffday : Offday
    {
        public DataOffday(OffdayReason reason, DateTime startDate, int duration) : base(reason, startDate, duration)
        {
        }
    }
}
