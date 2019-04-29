using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public string Adress { get; set; }
        public string Remark { get; set; }
        public int Area { get; set; }
        public int Amount { get; set; }
        public string perscription { get; set; }
        public DateTime Deadline { get; set; }
        public Customer Customer { get; set; }
        public AsphaltWork AsphaltWork { get; set; }
        public Machine Machine { get; set; }
        public DateTime StartDate { get; set; }

        public List<Assignment> assignments;

        private DateTime GetStartDate()
        {
            throw new NotImplementedException();
        }

        private DateTime GetNextAvailableDate()
        {
            throw new NotImplementedException();
        }
    }
}
