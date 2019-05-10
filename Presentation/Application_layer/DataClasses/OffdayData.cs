using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer.DataClasses
{
    public class OffdayData : Offday
    {
        internal OffdayData(OffdayReason reason, DateTime startDate, int duration) : base(reason, startDate, duration)
        {
        }
    }
}
