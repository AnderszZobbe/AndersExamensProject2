using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Controller
    {
        private List<Order> orders;
        private List<Workteam> workteams;
        private List<Offday> offdays;

        public Controller()
        {

        }

        public static Controller Instance { get; } = new Controller();

        public void SaveOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void SaveWorkteam(Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration)
        {
            throw new NotImplementedException();
        }

        public void Reschedule(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Order> ListOfOrdersFromDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
