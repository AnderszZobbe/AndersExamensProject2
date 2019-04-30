using Application_layer.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class Controller
    {
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();
        private ObservableCollection<Workteam> workteams = new ObservableCollection<Workteam>();

        public static Controller Instance { get; } = new Controller();

        public bool DeleteOrder(Order order)
        {
            return orders.Remove(order);
        }

        public void SaveOrder(Order order)
        {
            //ToDo add DBconnector method
            orders.Add(order);
        }

        public Workteam CreateWorkteam(string foreman)
        {
            if (foreman == "")
            {
                throw new ArgumentException("String argument for CreateWorkteam cannot be empty");
            }
            if (foreman == null)
            {
                throw new ArgumentNullException("String argument for CreateWorkteam cannot be null");
            }

            foreach (Workteam item in workteams)
            {
                if (item.Foreman == foreman)
                {
                    throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
                }
            }

            Workteam workteam = new Workteam()
            {
                Foreman = foreman
            };

            //ToDo add DBconnector method
            workteams.Add(workteam);

            return workteam;
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration, Workteam workteam)
        {
            Offday offday = new Offday(reason, startDate, duration);
            //ToDo add DBconnector method
            workteam.offdays.Add(offday);
            //ToDo Reschedule(date);
        }

        public ObservableCollection<Workteam> GetAllWorkteams()
        {
            return workteams;
        }

        public void Reschedule(DateTime date)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Order> ListOfOrdersFromDate(Workteam workteam, DateTime date)
        {
            //loo needs to be sorted 
            //List<Order> loo = new List<Order>();
            //foreach(Order i in orders)
            //{
            //    foreach (Assignment x in i.assignments)
            //    {
            //            if (i.LastDay(workteam) >= date)
            //            {
            //                loo.Add(i);
            //            }
            //            break;
            //    }
            //}
            //return loo
            throw new NotImplementedException();
        }

        public void EditForeman(string foremanName, Workteam workteam)
        {
            //ToDo add DBconnector method
            workteam.Foreman = foremanName;
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return workteams.Remove(workteam);
        }
    }
}
