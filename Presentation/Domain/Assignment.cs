using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Assignment
    {
        public Workform Workform { get; set; } = Workform.Dag;
        public int Duration { get; set; } = 0;
    }
}
