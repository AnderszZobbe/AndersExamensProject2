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

        private Controller()
        {
        }

        public static Controller Instance
        {
            get
            {
                instance = instance ?? new Controller();
                return instance;
            }
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            return Connector.DeleteOrder(workteam, order);
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            if (!Connector.WorkteamExists(workteam))
            {
                throw new ArgumentNullException("A valid workteam was not given");
            }

            if (Connector.GetAllOrders().Any(o => orderNumber.HasValue && o.OrderNumber == orderNumber))
            {
                throw new DuplicateObjectException("There already exists an order with that order number");
            }

            Order order = new Order()
            {
                OrderNumber = orderNumber,
                Address = address,
                Remark = remark,
                Area = area,
                Amount = amount,
                Prescription = prescription,
                Deadline = deadline
            };

            Connector.CreateOrder(workteam, order);

            return order;
        }

        public void SetStartDateOnOrder(Order order, DateTime startDate)
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

        public void EditOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
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

            int orderIndex = orders.IndexOf(orderToRescheduleFrom);

            SetStartDateOnOrder(orderToRescheduleFrom, startDate);

            for (int i = orderIndex; i < orders.Count - 1; i++)
            {
                Order order = orders[i];

                if (order.StartDate != null) // Is it assigned to the board?
                {
                    DateTime nextAvailableDate = workteam.GetNextAvailableDate(order);

                    SetStartDateOnOrder(orders[i + 1], nextAvailableDate);
                }
            }
        }
        
        public void EditForeman(Workteam workteam, string foreman)
        {
            if (foreman == string.Empty)
            {
                throw new ArgumentException();
            }

            if (Connector.GetAllWorkteams().Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException();
            }

            Connector.UpdateWorkteamForeman(workteam, foreman ?? throw new ArgumentNullException());
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return Connector.DeleteWorkteam(workteam);
        }

        public bool DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            Offday offday = workteam.GetOffday(date);

            return Connector.DeleteOffday(workteam, offday);
        }
    }
}
