using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class Controller
    {
        private List<Order> orders;
        private List<Workteam> workteams;

        public Controller()
        {

        }

        public static Controller Instance { get; } = new Controller();

        public void DeleteOrder(Order order)
        {
            if (!orders.Remove(order))
            {
                //ToDo throw exception
            }
        }

        public void SaveOrder(Order order)
        {
            //ToDo add DBconnector method
            orders.Add(order);
        }

        public void SaveWorkteam(Workteam workteam)
        {
            //ToDo add DBconnector method
            workteams.Add(workteam);
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration, Workteam workteam)
        {
            Offday offday = new Offday(reason, startDate, duration);
            //ToDo add DBconnector method
            workteam.AddOffDay(offday);
            //ToDo Reschedule(date);
        }

        public List<Workteam> GetAllWorkteams()
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

        public void EditForeman(string foremanName, Workteam workteam)
        {
            //ToDo add DBconnector method
            workteam.EditForman(foremanName);
        }

        public void DeleteWorkteam(Workteam workteam)
        {
            //ToDo add DBconnector method
            if (workteams.Contains(workteam))
            {
                workteams.Remove(workteam);
            }
            
        }
    }
}
