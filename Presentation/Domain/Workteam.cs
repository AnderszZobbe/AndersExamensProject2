using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Workteam
    {
        public string Foreman { get; set; }
        public List<Offday> offdays;

        public bool IsAnOffday(DateTime date)
        {
            foreach(Offday i in offdays)
            {
                if (i.IsDate(date))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
