using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Workteam
    {
        private string foreman;
        private List<Offday> offdays;

        private bool IsAnOffday()
        {
            throw new NotImplementedException();
        }
        public void EditForman(string Name)
        {
            foreman = Name;
        }
    }
}
