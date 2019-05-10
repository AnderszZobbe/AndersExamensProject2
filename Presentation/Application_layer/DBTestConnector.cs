using Application_layer;
using Application_layer.Exceptions;
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
        private readonly Dictionary<Order, int> orders = new Dictionary<Order, int>();
        private readonly Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();
        private readonly Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();
        private readonly Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

        private int orderID = 1;
        private int workteamID = 1;
        private int assignmentID = 1;
        private int offdayID = 1;

        public bool AssignmentExists(Assignment assignment)
        {
            return assignments.ContainsKey(assignment);
        }

        public Assignment CreateAssignment(Assignment assignment, Order order)
        {
            // All clear
            assignments.Add(assignment, assignmentID++);
            order.assignments.Add(assignment);
            return assignment;
        }

        public Offday CreateOffday(Offday offday, Workteam workteam)
        {
            // All clear
            offdays.Add(offday, offdayID++);
            workteam.offdays.Add(offday);
            return offday;
        }

        public Order CreateOrder(Order order, Workteam workteam)
        {
            // All clear
            orders.Add(order, orderID++);
            workteam.orders.Add(order);
            return order;
        }

        public Workteam CreateWorkteam(string foreman)
        {
            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            // All clear
            Workteam workteam = new Workteam(foreman);
            workteams.Add(workteam, workteamID++);
            return workteam;
        }

        public bool DeleteAssignment(Order order, Assignment assignment)
        {
            order.assignments.Remove(assignment);
            return assignments.Remove(assignment);
        }

        public bool DeleteOffday(Offday offday, Workteam workteam)
        {
            workteam.offdays.Remove(offday);
            return offdays.Remove(offday);
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            workteam.orders.Remove(order);
            return orders.Remove(order);
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return workteams.Remove(workteam);
        }

        public void FillOrderWithAssignments(Order order)
        {
        }

        public void FillWorkteamWithOffdays(Workteam workteam)
        {
        }

        public void FillWorkteamWithOrders(Workteam workteam)
        {
        }

        public List<Assignment> GetAllAssignments()
        {
            return assignments.Keys.ToList();
        }

        public List<Offday> GetAllOffdays()
        {
            return offdays.Keys.ToList();
        }

        public List<Order> GetAllOrders()
        {
            return orders.Keys.ToList();
        }

        public List<Workteam> GetAllWorkteams()
        {
            return workteams.Keys.ToList();
        }

        public bool OffdayExists(Offday offday)
        {
            return offdays.ContainsKey(offday);
        }

        public bool OrderExists(Order order)
        {
            return orders.ContainsKey(order);
        }

        public void UpdateOrderStartDate(Order order, DateTime startDate)
        {
            order.StartDate = startDate;
        }

        public void UpdateWorkteamForeman(Workteam workteam, string foreman)
        {
            if (foreman == string.Empty)
            {
                throw new ArgumentException();
            }

            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException();
            }

            workteam.Foreman = foreman ?? throw new ArgumentNullException();
        }

        public bool WorkteamExists(Workteam workteam)
        {
            return workteams.ContainsKey(workteam);
        }
    }
}
