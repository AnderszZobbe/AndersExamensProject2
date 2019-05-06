using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public readonly List<Assignment> assignments = new List<Assignment>();

        public int? OrderNumber { get; set; } = null;
        public string Address { get; set; } = null;
        public string Remark { get; set; } = null;
        public int? Area { get; set; } = null;
        public int? Amount { get; set; } = null;
        public string Prescription { get; set; } = null;
        public DateTime? Deadline { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public int Priority { get; set; } = 0;

        public int GetTotalDuration()
        {
            int totalDuration = 0;
            foreach (Assignment assignment in assignments)
            {
                if (totalDuration == 0)
                {
                    totalDuration++;
                }
                totalDuration += assignment.duration;
            }
            return totalDuration;
        }

        private DateTime GetNextAvailableDate()
        {
            throw new NotImplementedException();
        }

        public DateTime LastDay(Workteam workteam)
        {
            if (StartDate.HasValue)
            {
                DateTime lastDay = (DateTime)StartDate;
                foreach (Assignment i in assignments)
                {
                    for (int x = i.duration; x == 0; x = x - 1)
                    {
                        if (workteam.IsAnOffday(lastDay))
                        {
                            x++;
                        }
                        lastDay.AddDays(1);
                    }
                }
                return lastDay;
            }
            return DateTime.MinValue;
            
        }
    }
}
