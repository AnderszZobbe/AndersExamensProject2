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

        public virtual int? OrderNumber { get; set; }

        public virtual string Address { get; set; }

        public virtual string Remark { get; set; }

        public virtual int? Area { get; set; }

        public virtual int? Amount { get; set; }

        public virtual string Prescription { get; set; }

        public virtual DateTime? Deadline
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

        public virtual DateTime? StartDate
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

        public virtual string Customer { get; set; }

        public virtual string Machine { get; set; }

        public virtual string AsphaltWork { get; set; }
    }
}
