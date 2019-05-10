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
        public static IConnector Connector;

        private Controller()
        {
        }

        public static Controller Instance { get; } = new Controller();

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            return Connector.DeleteOrder(workteam, order);
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            // Init exceptions
            if (workteam == null)
            {
                throw new ArgumentNullException("A workteam was not given");
            }

            List<Order> orders = Connector.GetAllOrders().ToList<Order>();

            if (orders.Any(o => orderNumber != null && o.OrderNumber == orderNumber))
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

            //workteam.orders.Add(order);

            Connector.CreateOrder(order, workteam);

            return order;
        }

        public void SetStartDateOnOrder(Order order, DateTime startDate)
        {
            Connector.UpdateOrderStartDate(order, startDate);
        }

        public Assignment CreateAssignment(Order order, int? duration = null, Workform? workform = null)
        {
            if (order == null)
            {
                throw new ArgumentNullException("An order was not given");
            }
            if (!Connector.OrderExists(order))
            {
                throw new NullReferenceException("The order does not exists");
            }

            // Init exceptions
            if (order == null)
            {
                throw new ArgumentNullException("An order was not given");
            }
            if (duration < 0)
            {
                throw new DateOutOfRangeException("The given duration is negative");
            }
            if (duration > 360)
            {
                throw new DateOutOfRangeException("The given duration is longer that a year");
            }

            Assignment assignment = new Assignment();
            assignment.Workform = workform ?? assignment.Workform;
            assignment.Duration = duration ?? assignment.Duration;

            Connector.CreateAssignment(assignment, order);

            return assignment;
        }

        public void EditOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            List<Order> orders = Connector.GetAllOrders().ToList<Order>();

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
            return Connector.CreateWorkteam(foreman);
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration, Workteam workteam)
        {
            DateTime dateRoller = startDate;
            if (workteam == null)
            {
                throw new ArgumentNullException("You are trying to add offdaýs to a nonexistent workteam");
            }
            if (duration < 0)
            {
                throw new DateOutOfRangeException("Your are trying add an offdate with a duration of less than 0");
            }
            if (startDate < DateTime.Today)
            {
                throw new DateOutOfRangeException("You are trying to add a task, which starts in the past");
            }
            if (startDate > DateTime.Today.AddYears(1))
            {
                throw new DateOutOfRangeException("You are trying to add a task, which starts in a year");
            }
            for (int i = 0; i < duration; i++)
            {
                if (workteam.IsAnOffday(dateRoller))
                {
                    throw new OverlapException("There is another offday in the given period");
                }


                dateRoller = dateRoller.AddDays(1);
            }

            Offday offday = new Offday(reason, startDate, duration);
            workteam.offdays.Add(offday);
        }

        public List<Workteam> GetAllWorkteams()
        {
            return Connector.GetAllWorkteams().ToList<Workteam>();
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
            Connector.UpdateWorkteamForeman(workteam, foreman);
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return Connector.DeleteWorkteam(workteam);
        }

        public bool DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            Offday offday = workteam.GetOffday(date);

            return Connector.DeleteOffday(offday, workteam);
        }
    }
}
