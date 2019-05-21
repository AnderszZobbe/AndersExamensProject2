using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Offday
    {
        private int duration;
        private DateTime startDate;

        public Offday(OffdayReason reason, DateTime startDate, int duration)
        {
            OffdayReason = reason;
            StartDate = startDate;
            Duration = duration;
        }

        public virtual OffdayReason OffdayReason { get; set; }

        public virtual DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value.Date;
            }
        }

        public virtual int Duration
        {
            get => duration;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Duration is not allowed to be lower than 0");
                }
                duration = value;
            }
        }

        public bool IsOffday(DateTime date)
        {
            date = date.Date;
            return date >= StartDate && date <= StartDate.AddDays(Duration) ? true : false;
        }
    }
}
