using Domain.Exceptions;
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
            return offdays.Any(o => o.IsOffday(date));
        }

        public OffdayReason GetOffdayReason(DateTime date)
        {
            return offdays.Find(o => o.IsOffday(date)).OffdayReason;
        }

        public bool IsAWorkday(Order order, DateTime date)
        {
            if (order.StartDate == null) // Return false if there's no startdate
            {
                return false;
            }

            List<Assignment> assignments = order.assignments;

            DateTime dateRoller = order.StartDate.Value;

            foreach (Assignment assignment in assignments)
            {
                for (int i = 0; i <= assignment.Duration; i++) // Loop for remaining duration days
                {
                    while (IsAnOffday(dateRoller))
                    {
                        dateRoller = dateRoller.AddDays(1);
                    }

                    if (dateRoller.Date == date.Date)
                    {
                        return true;
                    }

                    dateRoller = dateRoller.AddDays(1);
                }
            }

            return false;
        }

        public Workform GetWorkform(Order order, DateTime date)
        {
            if (order.StartDate == null) // Return false if there's no startdate
            {
                throw new NullReferenceException("Order doesn't have a start date");
            }

            List<Assignment> assignments = order.assignments;

            DateTime dateRoller = order.StartDate.Value;

            foreach (Assignment assignment in assignments)
            {
                for (int i = 0; i <= assignment.Duration; i++) // Loop for remaining duration days
                {
                    while (IsAnOffday(dateRoller))
                    {
                        dateRoller = dateRoller.AddDays(1);
                    }

                    if (dateRoller.Date == date.Date)
                    {
                        return assignment.Workform;
                    }

                    dateRoller = dateRoller.AddDays(1);
                }
            }

            throw new NullReferenceException("There's no workform on this date");
        }
    }
}
