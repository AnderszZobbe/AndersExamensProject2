using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public readonly List<Assignment> assignments = new List<Assignment>();

        public int? OrderNumber { get; set; }

        public string Address { get; set; }

        public string Remark { get; set; }

        public int? Area { get; set; }

        public int? Amount { get; set; }

        public string Prescription { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
