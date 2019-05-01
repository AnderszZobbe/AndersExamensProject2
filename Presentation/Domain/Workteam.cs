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

        public Workteam(string foreman)
        {
            if (foreman == "")
            {
                throw new ArgumentException("String argument for CreateWorkteam cannot be empty");
            }

            Foreman = foreman ?? throw new ArgumentNullException("String argument for CreateWorkteam cannot be null");
        }

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
