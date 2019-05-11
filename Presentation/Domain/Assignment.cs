using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Assignment
    {
        private Workform workform;
        private int duration;

        public Assignment(Workform workform, int duration)
        {
            Workform = workform;
            Duration = duration;
        }

        public Workform Workform { get; set; }

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
    }
}
