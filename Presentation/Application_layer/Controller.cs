using Application_layer.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Application_layer
{
    public class Controller
    {
        private static Controller instance;
        public static IConnector Connector;
        private readonly Repository repository;

        private Controller()
        {
            repository = new Repository(Connector);
        }

        public static Controller Instance
        {
            get
            {
                instance = instance ?? new Controller();
                return instance;
            }
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            if (!Connector.WorkteamExists(workteam))
            {
                throw new ArgumentNullException("A valid workteam was not given");
            }

            if (Connector.GetAllOrders().Any(o => orderNumber.HasValue && o.OrderNumber == orderNumber))
            {
                throw new DuplicateObjectException("There already exists an order with that order number");
            }

            return Connector.CreateOrder(workteam, orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);
        }

        public void SetStartDateOnOrder(Order order, DateTime? startDate)
        {
            Connector.UpdateOrderStartDate(order, startDate);
        }

        public Assignment CreateAssignment(Order order, int duration, Workform workform)
        {
            if (!Connector.OrderExists(order))
            {
                throw new NullReferenceException("The order does not exists");
            }

            return Connector.CreateAssignment(order, workform, duration);
        }

        public void EditOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            List<Order> orders = Connector.GetAllOrders();

            if (orders.Any(o => o.OrderNumber == orderNumber && o != order))
            {
                throw new DuplicateObjectException("There already exists an order with that order number");
            }

            order.OrderNumber = orderNumber;
            order.Address = address;
            order.Remark = remark;
            order.Area = area;
            order.Amount = amount;
            order.Prescription = prescription;
            order.Deadline = deadline;
        }

        public void FillWorkteamWithOrders(Workteam workteam)
        {
            Connector.FillWorkteamWithOrders(workteam);
        }

        private void FillOrderWithAssignments(Order order)
        {
            Connector.FillOrderWithAssignments(order);
        }

        public Workteam CreateWorkteam(string foreman)
        {
            if (foreman == string.Empty)
            {
                throw new ArgumentException();
            }

            if (Connector.GetAllWorkteams().Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            return Connector.CreateWorkteam(foreman);
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            if (!Connector.WorkteamExists(workteam))
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

            return Connector.CreateOffday(workteam, reason, startDate, duration);
        }

        public List<Workteam> GetAllWorkteams()
        {
            return Connector.GetAllWorkteams();
        }

        public void Reschedule(Workteam workteam, Order orderToRescheduleFrom, DateTime startDate)
        {
            List<Order> orders = workteam.orders;

            if (!orders.Exists(o => o == orderToRescheduleFrom))
            {
                throw new NotFoundException("The order was not found in the workteam provided.");
            }

            SetStartDateOnOrder(orderToRescheduleFrom, startDate);

            DateTime nextAvailableDate = workteam.GetNextAvailableDate(orderToRescheduleFrom);

            for (int i = orders.IndexOf(orderToRescheduleFrom) + 1; i < orders.Count; i++)
            {
                Order currentOrder = orders[i];

                if (currentOrder.StartDate != null) // Is it assigned to the board?
                {
                    SetStartDateOnOrder(currentOrder, nextAvailableDate);

                    nextAvailableDate = workteam.GetNextAvailableDate(currentOrder);
                }
            }
        }
        
        public void EditForeman(Workteam workteam, string foreman)
        {
            if (foreman == string.Empty)
            {
                throw new ArgumentException();
            }

            if (Connector.GetAllWorkteams().Any(o => o != workteam && o.Foreman == foreman))
            {
                throw new DuplicateObjectException();
            }

            Connector.UpdateWorkteamForeman(workteam, foreman ?? throw new ArgumentNullException());
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return Connector.DeleteWorkteam(workteam);
        }

        public bool DeleteOffday(Workteam workteam, Offday offday)
        {
            return Connector.DeleteOffday(workteam, offday);
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            return Connector.DeleteOrder(workteam, order);
        }

        public bool DeleteAssignment(Order order, Assignment assignment)
        {
            return Connector.DeleteAssignment(order, assignment);
        }

        public bool DeleteAllAssignmentsFromOrder(Order order)
        {
            List<Assignment> assignments = order.assignments;

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
            if (!workteam.orders.Contains(firstOrder))
            {
                throw new ArgumentException("Order does not exist in workteam");
            }

            SwapOrders(workteam, firstOrder, workteam.orders[workteam.orders.IndexOf(firstOrder) - 1]);
        }

        public void MoveOrderDown(Workteam workteam, Order firstOrder)
        {
            if (!workteam.orders.Contains(firstOrder))
            {
                throw new ArgumentException("Order does not exist in workteam");
            }

            SwapOrders(workteam, firstOrder, workteam.orders[workteam.orders.IndexOf(firstOrder) + 1]);
        }

        private void SwapOrders(Workteam workteam, Order firstOrder, Order secondOrder)
        {
            // TODO: Move them around
            List<Order> orders = workteam.orders;

            int indexOfFirstOrder = orders.IndexOf(firstOrder);
            int indexOfSecondOrder = orders.IndexOf(secondOrder);

            orders[indexOfFirstOrder] = secondOrder;
            orders[indexOfSecondOrder] = firstOrder;

            Connector.SwitchOrdersPriority(firstOrder, secondOrder);
        }
    }
}
