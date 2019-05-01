using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public List<Assignment> assignments;

        public Order(int orderNumber, string adress, string remark, int area, int amount, string prescription, DateTime deadline, Customer customer, AsphaltWork asphaltWork, Machine machine, DateTime startDate)
        {
            OrderNumber = orderNumber;
            Adress = adress ?? string.Empty;
            Remark = remark ?? string.Empty;
            Area = area;
            Amount = amount;
            Prescription = prescription ?? string.Empty;
            Deadline = deadline;
            Customer = customer;
            AsphaltWork = asphaltWork;
            Machine = machine;
            StartDate = startDate;
        }

        public int OrderNumber { get; private set; }
        public string Adress { get; private set; }
        public string Remark { get; private set; }
        public int Area { get; private set; }
        public int Amount { get; private set; }
        public string Prescription { get; private set; }
        public DateTime Deadline { get; private set; }
        public Customer Customer { get; private set; }
        public AsphaltWork AsphaltWork { get; private set; }
        public Machine Machine { get; private set; }
        public DateTime StartDate { get; private set; }

        private DateTime GetNextAvailableDate()
        {
            throw new NotImplementedException();
        }
        public DateTime LastDay(Workteam workteam)
        {
            //DateTime lastDay = StartDate;
            //foreach(Assignment i in assignments)
            //{
            //    for (int x = i.duration; x == 0; x = x - 1)
            //    {
            //        if (i.workteam.IsAnOffday(lastDay))
            //        {
            //            x++;
            //        }
            //        lastDay.AddDays(1);
            //    }
            //}
            //return lastDay;
            throw new NotImplementedException();
        }
    }
}
