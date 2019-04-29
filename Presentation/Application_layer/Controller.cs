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
        private List<Order> orders;
        private List<Workteam> workteams;

        public Controller()
        {

        }

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
            return workteams;
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

        public bool DeleteWorkteam(Workteam workteam)
        {
            return workteams.Remove(workteam);
        }
    }
}
