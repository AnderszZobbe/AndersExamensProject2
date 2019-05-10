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

        public int? OrderNumber { get; set; } = null;
        public string Address { get; set; } = null;
        public string Remark { get; set; } = null;
        public int? Area { get; set; } = null;
        public int? Amount { get; set; } = null;
        public string Prescription { get; set; } = null;
        public DateTime? Deadline { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public int Priority { get; set; } = 0;
    }
}
