using Domain;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class Manager : IConnector
    {
        public static IDataProvider DataProvider;

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
            if (!OrderExists(order))
            {
                throw new ArgumentNullException("The order does not exists");
            }

            KeyValuePair<Assignment, int> assignmentData = DataProvider.CreateAssignment(orders[order], workform, duration);

            assignments.Add(assignmentData.Key, assignmentData.Value);

            order.assignments.Add(assignmentData.Key);

            return assignmentData.Key;
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            if (!WorkteamExists(workteam))
            {
                throw new ArgumentNullException("You are trying to add offdays to a nonexistent workteam");
            }

            DateTime dateRoller = startDate;
            for (int i = 0; i < duration; i++)
            {
                if (workteam.IsAnOffday(dateRoller))
                {
                    throw new OverlapException("There is another offday in the given period");
                }
                dateRoller = dateRoller.AddDays(1);
            }

            KeyValuePair<Offday, int> offdayData = DataProvider.CreateOffday(workteams[workteam], reason, startDate, duration);

            offdays.Add(offdayData.Key, offdayData.Value);

            workteam.Offdays.Add(offdayData.Key);

            return offdayData.Key;
        }

        public void Reschedule(Workteam workteam, Order orderToRescheduleFrom, DateTime startDate)
        {
            List<Order> orders = GetAllOrdersFromWorkteam(workteam);

            if (!orders.Exists(o => o == orderToRescheduleFrom))
            {
                throw new ArgumentException("The order was not found in the workteam provided.");
            }

            UpdateOrderStartDate(orderToRescheduleFrom, startDate);

            DateTime nextAvailableDate = workteam.GetNextAvailableDate(orderToRescheduleFrom);

            for (int i = orders.IndexOf(orderToRescheduleFrom) + 1; i < orders.Count; i++)
            {
                Order currentOrder = orders[i];

                if (currentOrder.StartDate != null) // Is it assigned to the board?
                {
                    UpdateOrderStartDate(currentOrder, nextAvailableDate);

                    nextAvailableDate = workteam.GetNextAvailableDate(currentOrder);
                }
            }
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            if (!WorkteamExists(workteam))
            {
                throw new ArgumentNullException("A valid workteam was not given");
            }

            if (GetAllOrdersFromWorkteam(workteam).Any(o => orderNumber.HasValue && o.OrderNumber == orderNumber))
            {
                throw new DuplicateObjectException("There already exists an order with that order number");
            }

            KeyValuePair<Order, int> orderData = DataProvider.CreateOrder(workteams[workteam], orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

            orders.Add(orderData.Key, orderData.Value);

            workteam.Orders.Add(orderData.Key);

            return orderData.Key;
        }

        public Workteam CreateWorkteam(string foreman)
        {
            if (foreman == null)
            {
                throw new ArgumentNullException();
            }

            if (foreman == string.Empty)
            {
                throw new ArgumentException();
            }

            if (GetAllWorkteams().Any(o => o.Foreman == foreman))
            {
                throw new ArgumentException("There already exsits a workteam with a foreman by that name");
            }

            KeyValuePair<Workteam, int> WorkteamData = DataProvider.CreateWorkteam(foreman);

            workteams.Add(WorkteamData.Key, WorkteamData.Value);

            return WorkteamData.Key;
        }

        public bool DeleteAssignment(Order order, Assignment assignment)
        {
            if (!GetAllAssignmentsFromOrder(order).Contains(assignment))
            {
                return false;
            }

            DataProvider.DeleteAssignment(assignments[assignment]);

            order.assignments.Remove(assignment);

            return assignments.Remove(assignment);
        }

        public bool DeleteOffday(Workteam workteam, Offday offday)
        {
            if (!GetAllOffdaysFromWorkteam(workteam).Contains(offday))
            {
                return false;
            }

            DataProvider.DeleteOffday(offdays[offday]);

            workteam.Offdays.Remove(offday);

            return offdays.Remove(offday);
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            if (!GetAllOrdersFromWorkteam(workteam).Contains(order))
            {
                return false;
            }

            DataProvider.DeleteOrder(orders[order]);

            order.assignments.ForEach(o => assignments.Remove(o));

            workteam.Orders.Remove(order);

            return orders.Remove(order);
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            if (!GetAllWorkteams().Contains(workteam))
            {
                return false;
            }

            DataProvider.DeleteWorkteam(workteams[workteam]);

            workteam.Orders.ForEach(o => o.assignments.ForEach(p => assignments.Remove(p)));

            workteam.Orders.ForEach(o => orders.Remove(o));

            workteam.Offdays.ForEach(o => offdays.Remove(o));

            return workteams.Remove(workteam);
        }

        public List<Assignment> GetAllAssignmentsFromOrder(Order order)
        {
            Dictionary<Assignment, int> assignmentsData = DataProvider.GetAllAssignmentsByOrder(orders[order]);

            order.assignments.Clear();

            foreach (KeyValuePair<Assignment, int> assignmentData in assignmentsData)
            {
                if (!assignments.ContainsValue(assignmentData.Value)) // Add new ones to the repo
                {
                    assignments.Add(assignmentData.Key, assignmentData.Value);
                }

                order.assignments.Add(assignments.First(o => o.Value == assignmentData.Value).Key);
            }

            return order.assignments;
        }

        public List<Offday> GetAllOffdaysFromWorkteam(Workteam workteam)
        {
            Dictionary<Order, int> ordersData = DataProvider.GetAllOrdersByWorkteam(workteams[workteam]);

            workteam.Orders.Clear();

            foreach (KeyValuePair<Order, int> orderData in ordersData)
            {
                if (!orders.ContainsValue(orderData.Value)) // Add new ones to the repo
                {
                    orders.Add(orderData.Key, orderData.Value);
                }

                workteam.Orders.Add(orders.First(o => o.Value == orderData.Value).Key);
            }

            return workteam.Offdays;
        }

        public List<Order> GetAllOrdersFromWorkteam(Workteam workteam)
        {
            Dictionary<Order, int> ordersData = DataProvider.GetAllOrdersByWorkteam(workteams[workteam]);

            workteam.Orders.Clear();

            foreach (KeyValuePair<Order, int> orderData in ordersData)
            {
                if (!orders.ContainsValue(orderData.Value)) // Add new ones to the repo
                {
                    orders.Add(orderData.Key, orderData.Value);
                }

                workteam.Orders.Add(orders.First(o => o.Value == orderData.Value).Key);
            }

            return workteam.Orders;
        }

        public List<Workteam> GetAllWorkteams()
        {
            Dictionary<Workteam, int> workteamsData = DataProvider.GetAllWorkteams();

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
            DataProvider.SwapOrderPriorities(orders[firstOrder], orders[secondOrder]);

            GetAllOrdersFromWorkteam(workteam);
        }

        public void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            Workteam workteam = workteams.First(o => o.Key.Orders.Contains(order)).Key;

            if (GetAllOrdersFromWorkteam(workteam).Any(o => o != order && o.OrderNumber == orderNumber))
            {
                throw new ArgumentException("There already exists an order with that ordernumber!");
            }

            int orderID = orders[order];

            DataProvider.UpdateOrderOrderNumber(orderID, orderNumber);
            DataProvider.UpdateOrderAddress(orderID, address);
            DataProvider.UpdateOrderRemark(orderID, remark);
            DataProvider.UpdateOrderArea(orderID, area);
            DataProvider.UpdateOrderAmount(orderID, amount);
            DataProvider.UpdateOrderPrescription(orderID, prescription);
            DataProvider.UpdateOrderDeadline(orderID, deadline);
            DataProvider.UpdateOrderStartDate(orderID, startDate);
            DataProvider.UpdateOrderCustomer(orderID, customer);
            DataProvider.UpdateOrderMachine(orderID, machine);
            DataProvider.UpdateOrderAsphaltWork(orderID, asphaltWork);

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
            DataProvider.UpdateOrderStartDate(orders[order], startDate);
            order.StartDate = startDate;
        }

        public void UpdateWorkteam(Workteam workteam, string foreman)
        {
            if (GetAllWorkteams().Any(o => o != workteam && o.Foreman == foreman))
            {
                throw new ArgumentException("There already exists a workteam with that foreman");
            }

            workteam.Foreman = foreman;

            DataProvider.UpdateWorkteamForeman(workteams[workteam], foreman);
        }

        public bool WorkteamExists(Workteam workteam)
        {
            return workteams.Keys.Any(o => o == workteam);
        }

        public bool DeleteAllAssignmentsFromOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("Order was null");
            }

            if (!OrderExists(order))
            {
                throw new ArgumentException("The order does not exist");
            }

            List<Assignment> assignments = GetAllAssignmentsFromOrder(order).ToList();

            bool allAssignmentsDeleted = true;

            foreach (Assignment assignment in assignments)
            {
                allAssignmentsDeleted = DeleteAssignment(order, assignment);

                if (!allAssignmentsDeleted)
                {
                    return allAssignmentsDeleted;
                }
            }

            return allAssignmentsDeleted;
        }

        public bool DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            Offday offday = workteam.GetOffday(date);

            return DeleteOffday(workteam, offday);
        }

        public void MoveOrderUp(Workteam workteam, Order firstOrder)
        {
            List<Order> orders = GetAllOrdersFromWorkteam(workteam);

            if (!orders.Contains(firstOrder))
            {
                throw new ArgumentException("Order does not exist in workteam");
            }

            SwapOrdersPriority(workteam, firstOrder, orders[orders.IndexOf(firstOrder) - 1]);
        }

        public void MoveOrderDown(Workteam workteam, Order firstOrder)
        {
            if (!GetAllOrdersFromWorkteam(workteam).Contains(firstOrder))
            {
                throw new ArgumentException("Order does not exist in workteam");
            }

            SwapOrdersPriority(workteam, firstOrder, GetAllOrdersFromWorkteam(workteam)[GetAllOrdersFromWorkteam(workteam).IndexOf(firstOrder) + 1]);
        }
    }
}
