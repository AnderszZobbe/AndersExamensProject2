﻿using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DBTestConnector : IConnector
    {
        private int orderID = 1;
        private int workteamID = 1;
        private int assignmentID = 1;

        public void CreateAssignment(Dictionary<Assignment, int> assignments, Assignment assignment)
        {
            assignments.Add(assignment, assignmentID++);
        }

        public void CreateOrder(Dictionary<Order, int> orders, Order order)
        {
            orders.Add(order, orderID++);
        }

        public void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam)
        {
            workteams.Add(workteam, workteamID++);
        }

        public void GetAllAssignemntsByOrder(Dictionary<Order, int> orders, Workteam workteam)
        {
        }

        public void FillWorkteamWithOrders(Dictionary<Order, int> orders, Workteam workteam)
        {
        }

        public void GetAllWorkteams(Dictionary<Workteam, int> workteams)
        {
        }

        public void DeleteOffday(Dictionary<Offday, int> offdays, Offday offday)
        {
            offdays.Remove(offday);
        }

        public void CreateOffday(Dictionary<Offday, int> offdays, Offday offday)
        {
            throw new NotImplementedException();
        }

        public void FillWorkteamWithOffdays(Dictionary<Offday, int> offdays, Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public void FillOrderWithAssignments(Dictionary<Assignment, int> assignments, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
