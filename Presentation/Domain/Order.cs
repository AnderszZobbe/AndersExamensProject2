using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        private int orderNumber;
        private string address;
        private string remark;
        private int area;
        private int amount;
        private string prescription;
        private DateTime deadline;
        private Customer customer;
        private AsphaltWork asphaltWork;
        private Machine machine;
        private Workteam workteam;
        private List<Assignment> assignments;
        public DateTime startDate;

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
