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

        public string Foreman { get; set; }

        public DateTime GetNextAvailableDate(Order order)
        {
            if (!orders.Exists(o => o == order))
            {
                throw new NullReferenceException("The order provided was not found in this workteam.");
            }

            DateTime dateRoller = order.StartDate ?? throw new NullReferenceException("The startdate of the order is not found");

            bool nextAvailableDateFound = false;
            while (!nextAvailableDateFound)
            {
                if (!IsAnOffday(dateRoller) && !IsAWorkday(order, dateRoller))
                {
                    nextAvailableDateFound = true;
                }
                else
                {
                    dateRoller = dateRoller.AddDays(1);
                }
            }

            return dateRoller;
        }

        public bool IsAnOffday(DateTime date)
        {
            return offdays.Any(o => o.IsOffday(date));
        }

        public Offday GetOffday(DateTime date)
        {
            return offdays.Find(o => o.IsOffday(date));
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

        public bool IsThereAHigherPriorityOrderWithAStartDate(Order order)
        {
            throw new NotImplementedException();
        }

        public Order GetNextHigherPriorityOrderWithAStartDate(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
