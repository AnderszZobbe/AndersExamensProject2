using Domain.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class TestDataProvider : IDataProvider
    {
        private List<KeyValuePair<Order, int>> orders = new List<KeyValuePair<Order, int>>();
        private Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();
        private Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();
        private Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

        private int orderID;
        private int workteamID;
        private int assignmentID;
        private int offdayID;

        public KeyValuePair<Assignment, int> CreateAssignment(int order, Workform workform, int duration)
        {
            Assignment assignment = new Assignment(workform, duration);

            assignments.Add(assignment, ++assignmentID);

            return assignments.First(o => o.Key == assignment);
        }

        public KeyValuePair<Offday, int> CreateOffday(int workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            Offday offday = new Offday(reason, startDate, duration);

            offdays.Add(offday, ++offdayID);

            return offdays.First(o => o.Key == offday);
        }

        public KeyValuePair<Order, int> CreateOrder(int workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            Order order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

            var pair = new KeyValuePair<Order, int>(order, ++orderID);

            orders.Add(pair);

            return pair;
        }

        public KeyValuePair<Workteam, int> CreateWorkteam(string foreman)
        {
            Workteam workteam = new Workteam(foreman);

            workteams.Add(workteam, ++workteamID);

            return workteams.First(o => o.Key == workteam);
        }

        public void DeleteAssignment(int assignmentID)
        {
            Assignment assignment = assignments.First(o => o.Value == assignmentID).Key;

            assignments.Remove(assignment);
        }

        public void DeleteOffday(int offdayID)
        {
            Offday offday = offdays.First(o => o.Value == offdayID).Key;

            offdays.Remove(offday);
        }

        public void DeleteOrder(int orderID)
        {
            KeyValuePair<Order, int> pair = orders.First(o => o.Value == orderID);

            foreach (Assignment assignment in pair.Key.assignments)
            {
                DeleteAssignment(assignments[assignment]);
            }

            orders.Remove(pair);
        }

        public void DeleteWorkteam(int workteamID)
        {
            Workteam workteam = workteams.First(o => o.Value == workteamID).Key;

            foreach (Offday offday in workteam.Offdays)
            {
                DeleteOffday(offdays[offday]);
            }

            foreach (Order order in workteam.Orders)
            {
                DeleteOrder(orders.Find(o => o.Key == order).Value);
            }

            workteams.Remove(workteam);
        }

        public Dictionary<Assignment, int> GetAllAssignmentsByOrder(int order)
        {
            List<Assignment> assignments = orders.First(o => o.Value == order).Key.assignments;

            Dictionary<Assignment, int> assignmentIdPairs = new Dictionary<Assignment, int>();

            foreach (Assignment assignment in assignments)
            {
                assignmentIdPairs.Add(assignment, this.assignments[assignment]);
            }

            return assignmentIdPairs;
        }

        public Dictionary<Offday, int> GetAllOffdaysByWorkteam(int workteam)
        {
            List<Offday> offdays = workteams.First(o => o.Value == workteam).Key.Offdays;

            Dictionary<Offday, int> offdayIdPairs = new Dictionary<Offday, int>();

            foreach (Offday offday in offdays)
            {
                offdayIdPairs.Add(offday, this.offdays[offday]);
            }

            return offdayIdPairs;
        }

        public Dictionary<Order, int> GetAllOrdersByWorkteam(int workteam)
        {
            List<KeyValuePair<Order, int>> orders = this.orders.FindAll(o => workteams.First(p => p.Value == workteam).Key.Orders.Contains(o.Key));

            Dictionary<Order, int> keyValuePairs = new Dictionary<Order, int>();

            foreach (KeyValuePair<Order, int> order in orders)
            {
                keyValuePairs.Add(order.Key, order.Value);
            }

            return keyValuePairs;
        }

        public Dictionary<Workteam, int> GetAllWorkteams()
        {
            return workteams;
        }

        public void SwapOrderPriorities(int firstOrderID, int secondOrderID)
        {
            KeyValuePair<Order, int> firstOrderPair = orders.First(o => o.Value == firstOrderID);
            KeyValuePair<Order, int> secondOrderPair = orders.First(o => o.Value == secondOrderID);

            int firstOrderIndex = orders.IndexOf(firstOrderPair);
            int secondOrderIndex = orders.IndexOf(secondOrderPair);

            orders[firstOrderIndex] = secondOrderPair;
            orders[secondOrderIndex] = firstOrderPair;
        }

        public void UpdateOrderAddress(int order, string address)
        {
        }

        public void UpdateOrderAmount(int order, int? amount)
        {
        }

        public void UpdateOrderArea(int order, int? area)
        {
        }

        public void UpdateOrderAsphaltWork(int order, string asphaltWork)
        {
        }

        public void UpdateOrderCustomer(int order, string customer)
        {
        }

        public void UpdateOrderDeadline(int order, DateTime? deadline)
        {
        }

        public void UpdateOrderMachine(int order, string machine)
        {
        }

        public void UpdateOrderOrderNumber(int order, int? orderNumber)
        {
        }

        public void UpdateOrderPrescription(int order, string prescription)
        {
        }

        public void UpdateOrderRemark(int order, string remark)
        {
        }

        public void UpdateOrderStartDate(int order, DateTime? startDate)
        {
        }

        public void UpdateWorkteamForeman(int workteam, string foreman)
        {
        }
    }
}
