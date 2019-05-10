using Application_layer.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Application_layer.DataClasses;

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
            if (workteam == null )
            {
                throw new ArgumentNullException("Workteam is nonexistent");
            }
            if (order == null)
            {
                throw new ArgumentNullException("Order is nonexistent");
            }
            if (workteam.orders.Remove(order))
            {
                Connector.DeleteOrder(workteam as WorkteamData, order as OrderData);
                return true;
            }
            else
            {
                workteam.orders.Add(order);
                return false;
            }
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

            Order order = new OrderData()
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

            Connector.CreateOrder(order as OrderData, workteam as WorkteamData);

            return order;
        }

        public void SetStartDateOnOrder(Order order, DateTime startDate)
        {
            Connector.UpdateOrderStartDate(order as OrderData, startDate);
        }

        public Assignment CreateAssignment(Order order, int? duration = null, Workform? workform = null)
        {
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

            Assignment assignment = new AssignmentData();
            assignment.Workform = workform ?? assignment.Workform;
            assignment.Duration = duration ?? assignment.Duration;

            Connector.CreateAssignment(assignment as AssignmentData, order as OrderData);

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

        public List<Order> GetAllOrdersByWorkteam(Workteam workteam)
        {
            Connector.FillWorkteamWithOrders(workteam as WorkteamData);

            return workteam.orders;
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

            Offday offday = new OffdayData(reason, startDate, duration);
            workteam.offdays.Add(offday);
        }

        private void FillOrderWithAssignments(Order order)
        {
            Connector.FillOrderWithAssignments(order as OrderData);
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
        
        public void EditForeman(string foremanName, Workteam workteam)
        {
            List<Workteam> workteams = Connector.GetAllWorkteams().ToList<Workteam>();

            if (workteams.Where(o => o.Foreman == foremanName).Count() != 0)
            {
                throw new DuplicateObjectException();
            }
            if (foremanName.Count() == 0)
            {
                throw new ArgumentException();
            }
            //ToDo add DBconnector method
            workteam.Foreman = foremanName ?? throw new ArgumentNullException();
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return Connector.DeleteWorkteam(workteam as WorkteamData);
        }

        public void DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            Offday offday = workteam.GetOffday(date);

            if (workteam.offdays.Remove(offday))
            {
                Connector.DeleteOffday(offday as OffdayData, workteam as WorkteamData);
            }
            else
            {
                throw new OffdayNotFoundException();
            }
        }
    }
}
