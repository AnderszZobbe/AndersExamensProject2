using Application_layer;
using Application_layer.DataClasses;
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
        private readonly Dictionary<OrderData, int> orders = new Dictionary<OrderData, int>();
        private readonly Dictionary<WorkteamData, int> workteams = new Dictionary<WorkteamData, int>();
        private readonly Dictionary<AssignmentData, int> assignments = new Dictionary<AssignmentData, int>();
        private readonly Dictionary<OffdayData, int> offdays = new Dictionary<OffdayData, int>();

        private int orderID = 1;
        private int workteamID = 1;
        private int assignmentID = 1;
        private int offdayID = 1;

        public AssignmentData CreateAssignment(AssignmentData assignment, OrderData order)
        {
            // All clear
            assignments.Add(assignment, assignmentID++);
            order.assignments.Add(assignment);
            return assignment;
        }

        public OffdayData CreateOffday(OffdayData offday, WorkteamData workteam)
        {
            // All clear
            offdays.Add(offday, offdayID++);
            workteam.offdays.Add(offday);
            return offday;
        }

        public OrderData CreateOrder(OrderData order, WorkteamData workteam)
        {
            // All clear
            orders.Add(order, orderID++);
            workteam.orders.Add(order);
            return order;
        }

        public WorkteamData CreateWorkteam(string foreman)
        {
            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            // All clear
            WorkteamData workteam = new WorkteamData(foreman);
            workteams.Add(workteam, workteamID++);
            return workteam;
        }

        public bool DeleteAssignment(OrderData order, AssignmentData assignment)
        {
            order.assignments.Remove(assignment);
            return assignments.Remove(assignment);
        }

        public bool DeleteOffday(OffdayData offday, WorkteamData workteam)
        {
            workteam.offdays.Remove(offday);
            return offdays.Remove(offday);
        }

        public bool DeleteOrder(WorkteamData workteam, OrderData order)
        {
            workteam.orders.Remove(order);
            return orders.Remove(order);
        }

        public bool DeleteWorkteam(WorkteamData workteam)
        {
            return workteams.Remove(workteam);
        }

        public void FillOrderWithAssignments(OrderData order)
        {
        }

        public void FillWorkteamWithOffdays(WorkteamData workteam)
        {
        }

        public void FillWorkteamWithOrders(WorkteamData workteam)
        {
        }

        public List<AssignmentData> GetAllAssignments()
        {
            return assignments.Keys.ToList();
        }

        public List<OffdayData> GetAllOffdays()
        {
            return offdays.Keys.ToList();
        }

        public List<OrderData> GetAllOrders()
        {
            return orders.Keys.ToList();
        }

        public List<WorkteamData> GetAllWorkteams()
        {
            return workteams.Keys.ToList();
        }

        public void UpdateOrderStartDate(OrderData order, DateTime startDate)
        {
            order.StartDate = startDate;
        }
    }
}
