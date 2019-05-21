using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        private DateTime? deadline;
        private DateTime? startDate;

        public List<Assignment> assignments { get; } = new List<Assignment>();

        public Order(int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            OrderNumber = orderNumber;
            Address = address;
            Remark = remark;
            Area = area;
            Amount = amount;
            Prescription = prescription;
            Deadline = deadline;
            StartDate = startDate;
            Customer = customer;
            Machine = machine;
            AsphaltWork = asphaltWork;
        }

        public int? OrderNumber { get; set; }

        public string Address { get; set; }

        public string Remark { get; set; }

        public int? Area { get; set; }

        public int? Amount { get; set; }

        public string Prescription { get; set; }

        public DateTime? Deadline
        {
            get => deadline;
            set
            {
                if (value.HasValue)
                {
                    deadline = value.Value.Date;
                }
                else
                {
                    deadline = null;
                }
            }
        }

        public DateTime? StartDate
        {
            get => startDate;
            set
            {
                if (value.HasValue)
                {
                    startDate = value.Value.Date;
                }
                else
                {
                    startDate = null;
                }
            }
        }

        public string Customer { get; set; }

        public string Machine { get; set; }

        public string AsphaltWork { get; set; }
    }
}
