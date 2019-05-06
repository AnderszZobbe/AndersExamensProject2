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
        public readonly List<Offday> offdays = new List<Offday>();
        public readonly List<Order> orders = new List<Order>();

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
            foreach(Offday offday in offdays)
            {
                if (offday.IsOffday(date))
                {
                    return true;
                }
            }
            return false;
        }

        public OffdayReason GetOffdayReason(DateTime date)
        {
            foreach (Offday offday in offdays)
            {
                if (offday.IsOffday(date))
                {
                    return offday.OffdayReason;
                }
            }
            throw new Exception(); // TODO: Put notfound exception in the domain layer
        }

        public bool IsAWorkday(Order order, DateTime date)
        {
            // TODO: MAKE
            return true;
        }

        public Workform GetWorkform(Order order, DateTime date)
        {
            // TODO: MAKE
            return Workform.Day;
        }
    }
}
