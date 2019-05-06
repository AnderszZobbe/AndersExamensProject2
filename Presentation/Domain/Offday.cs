﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Offday
    {
        public readonly OffdayReason OffdayReason;
        public readonly DateTime StartDate;
        public readonly int Duration;

        public Offday(OffdayReason reason, DateTime startDate, int duration)
        {
            OffdayReason = reason;
            StartDate = startDate;
            Duration = duration;
        }

        public bool IsOffday(DateTime date)
        {
            if(date >= StartDate && date <= StartDate.AddDays(Duration))
            {
                return true;
            }
            return false;
        }
    }
}
