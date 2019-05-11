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

        public OffdayReason OffdayReason { get; set; }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                value = value.Date;
                if (value < DateTime.Today)
                {
                    throw new ArgumentOutOfRangeException("Duration is not allowed to be lower than current day");
                }
                if (value > DateTime.Today.AddYears(1))
                {
                    throw new ArgumentOutOfRangeException("Duration is not allowed to be more than a year from current day");
                }
                startDate = value;
            }
        }

        public int Duration
        {
            get => duration;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Duration is not allowed to be lower than 0");
                }
                if (value > 365)
                {
                    throw new ArgumentOutOfRangeException("Duration is not allowed to be higher than 365");
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
