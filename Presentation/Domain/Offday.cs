using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Offday
    {
        private OffdayReason reason;
        private DateTime startDate;
        private int duration;

        public Offday(OffdayReason reason, DateTime startDate, int duration)
        {
            this.reason = reason;
            this.startDate = startDate;
            this.duration = duration;
        }
    }
}
