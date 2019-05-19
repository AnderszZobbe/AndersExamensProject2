using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataClasses
{
    public class DataOrder : Order
    {
        public DataOrder(int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork) : base(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork)
        {
        }
    }
}
