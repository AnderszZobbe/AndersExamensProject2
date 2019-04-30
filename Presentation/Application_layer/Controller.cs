﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class Controller
    {
        private List<Order> orders = new List<Order>();
        private List<Workteam> workteams = new List<Workteam>();

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
                    throw new ArgumentException("There already exsits a workteam with a foreman by that name");
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

        public List<Workteam> GetAllWorkteams()
        {
            return workteams;
        }

        public void Reschedule(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Order> ListOfOrdersFromDate(Workteam workteam, DateTime date)
        {
            //loo needs to be sorted 
            //List<Order> loo = new List<Order>();
            //foreach(Order i in orders)
            //{
            //    foreach (Assignment x in i.assignments)
            //    {
            //        if(x.workteam == workteam)
            //        {
            //            if (i.LastDay >= date)
            //            {
            //                loo.Add(i);
            //            }
            //            break;
            //        }
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
