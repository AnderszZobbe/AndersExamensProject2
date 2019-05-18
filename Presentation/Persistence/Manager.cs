using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class Manager : IConnector
    {
        private readonly DataProvider dataProvider = new DataProvider();

        private readonly Dictionary<Order, int> orders = new Dictionary<Order, int>();
        private readonly Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();
        private readonly Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();
        private readonly Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

        public bool AssignmentExists(Assignment assignment)
        {
            return assignments.Keys.Any(o => o == assignment);
        }

        public Assignment CreateAssignment(Order order, Workform workform, int duration)
        {
            KeyValuePair<Assignment, int> assignmentData = dataProvider.CreateAssignment(orders[order], workform, duration);

            assignments.Add(assignmentData.Key, assignmentData.Value);

            order.assignments.Add(assignmentData.Key);

            return assignmentData.Key;
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            KeyValuePair<Offday, int> offdayData = dataProvider.CreateOffday(workteams[workteam], reason, startDate, duration);

            offdays.Add(offdayData.Key, offdayData.Value);

            workteam.Offdays.Add(offdayData.Key);

            return offdayData.Key;
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            KeyValuePair<Order, int> orderData = dataProvider.CreateOrder(workteams[workteam], orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

            orders.Add(orderData.Key, orderData.Value);

            workteam.Orders.Add(orderData.Key);

            return orderData.Key;
        }

        public Workteam CreateWorkteam(string foreman)
        {
            KeyValuePair<Workteam, int> WorkteamData = dataProvider.CreateWorkteam(foreman);

            workteams.Add(WorkteamData.Key, WorkteamData.Value);

            return WorkteamData.Key;
        }

        public bool DeleteAssignment(Order order, Assignment assignment)
        {
            dataProvider.DeleteAssignment(assignments[assignment]);

            order.assignments.Remove(assignment);

            return assignments.Remove(assignment);
        }

        public bool DeleteOffday(Workteam workteam, Offday offday)
        {
            dataProvider.DeleteOffday(offdays[offday]);

            workteam.Offdays.Remove(offday);

            return offdays.Remove(offday);
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            dataProvider.DeleteOrder(orders[order]);

            order.assignments.ForEach(o => assignments.Remove(o));

            workteam.Orders.Remove(order);

            return orders.Remove(order);
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            dataProvider.DeleteWorkteam(workteams[workteam]);

            workteam.Orders.ForEach(o => o.assignments.ForEach(p => assignments.Remove(p)));

            workteam.Orders.ForEach(o => orders.Remove(o));

            workteam.Offdays.ForEach(o => offdays.Remove(o));

            return workteams.Remove(workteam);
        }

        public void FillOrderWithAssignments(Order order)
        {
            Dictionary<Assignment, int> assignmentsData = dataProvider.GetAllAssignmentsByOrder(orders[order]);

            order.assignments.Clear();

            foreach (KeyValuePair<Assignment, int> assignmentData in assignmentsData)
            {
                if (!assignments.ContainsValue(assignmentData.Value)) // Add new ones to the repo
                {
                    assignments.Add(assignmentData.Key, assignmentData.Value);
                }

                order.assignments.Add(assignments.First(o => o.Value == assignmentData.Value).Key);
            }
        }

        public void FillWorkteamWithOffdays(Workteam workteam)
        {
            Dictionary<Offday, int> offdaysData = dataProvider.GetAllOffdaysByWorkteam(workteams[workteam]);

            workteam.Offdays.Clear();

            foreach (KeyValuePair<Offday, int> offdayData in offdaysData)
            {
                if (!offdays.ContainsValue(offdayData.Value)) // Add new ones to the repo
                {
                    offdays.Add(offdayData.Key, offdayData.Value);
                }

                workteam.Offdays.Add(offdays.First(o => o.Value == offdayData.Value).Key);
            }
        }

        public void FillWorkteamWithOrders(Workteam workteam)
        {
            Dictionary<Order, int> ordersData = dataProvider.GetAllOrdersByWorkteam(workteams[workteam]);

            workteam.Orders.Clear(); 

            foreach (KeyValuePair<Order, int> orderData in ordersData)
            {
                if (!orders.ContainsValue(orderData.Value)) // Add new ones to the repo
                {
                    orders.Add(orderData.Key, orderData.Value);
                }

                workteam.Orders.Add(orders.First(o => o.Value == orderData.Value).Key);
            }
        }

        public List<Assignment> GetAllAssignments()
        {
            throw new NotImplementedException();
        }

        public List<Assignment> GetAllAssignmentsFromOrder(Order order)
        {
            FillOrderWithAssignments(order);

            return order.assignments;
        }

        public List<Offday> GetAllOffdays()
        {
            throw new NotImplementedException();
        }

        public List<Offday> GetAllOffdaysFromWorkteam(Workteam workteam)
        {
            FillWorkteamWithOffdays(workteam);

            return workteam.Offdays;
        }

        public List<Order> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAllOrdersFromWorkteam(Workteam workteam)
        {
            FillWorkteamWithOrders(workteam);

            return workteam.Orders;
        }

        public List<Workteam> GetAllWorkteams()
        {
            Dictionary<Workteam, int> workteamsData = dataProvider.GetAllWorkteams();

            foreach (KeyValuePair<Workteam, int> workteamData in workteamsData)
            {
                if (!workteams.ContainsValue(workteamData.Value)) // Add new ones to the repo
                {
                    workteams.Add(workteamData.Key, workteamData.Value);
                }
            }

            workteams.OrderBy(o => o.Value);
            //workteams.Keys.ToList().Sort();

            return workteams.Keys.ToList();
        }

        public bool OffdayExists(Offday offday)
        {
            return offdays.Keys.Any(o => o == offday);
        }

        public bool OrderExists(Order order)
        {
            return orders.Keys.Any(o => o == order);
        }

        public void SwapOrdersPriority(Workteam workteam, Order firstOrder, Order secondOrder)
        {
            dataProvider.SwapOrderPriorities(orders[firstOrder], orders[secondOrder]);

            FillWorkteamWithOrders(workteam);
        }

        public void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            int orderID = orders[order];

            dataProvider.UpdateOrderOrderNumber(orderID, orderNumber);
            dataProvider.UpdateOrderAddress(orderID, address);
            dataProvider.UpdateOrderRemark(orderID, remark);
            dataProvider.UpdateOrderArea(orderID, area);
            dataProvider.UpdateOrderAmount(orderID, amount);
            dataProvider.UpdateOrderPrescription(orderID, prescription);
            dataProvider.UpdateOrderDeadline(orderID, deadline);
            dataProvider.UpdateOrderStartDate(orderID, startDate);
            dataProvider.UpdateOrderCustomer(orderID, customer);
            dataProvider.UpdateOrderMachine(orderID, machine);
            dataProvider.UpdateOrderAsphaltWork(orderID, asphaltWork);

            order.OrderNumber = orderNumber;
            order.Address = address;
            order.Remark = remark;
            order.Area = area;
            order.Amount = amount;
            order.Prescription = prescription;
            order.Deadline = deadline;
            order.StartDate = startDate;
            order.Customer = customer;
            order.Machine = machine;
            order.AsphaltWork = asphaltWork;
        }

        public void UpdateOrderStartDate(Order order, DateTime? startDate)
        {
            dataProvider.UpdateOrderStartDate(orders[order], startDate);
            order.StartDate = startDate;
        }

        public void UpdateWorkteam(Workteam workteam, string foreman)
        {
            dataProvider.UpdateWorkteamForeman(workteams[workteam], foreman);
            workteam.Foreman = foreman;
        }

        public bool WorkteamExists(Workteam workteam)
        {
            return workteams.Keys.Any(o => o == workteam);
        }
    }
}
