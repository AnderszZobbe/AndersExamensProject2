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

        private readonly Dictionary<Order, int> orders = new Dictionary<Order, int>();
        private readonly Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();
        private readonly Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();
        private readonly Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

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
                //Connector.DeleteOrder(orders, workteam, order);
                return true;
            }
            else
            {
                workteam.orders.Add(order);
                return false;
            }
        }

        public void SetOrderStartDate(Order order, DateTime startDate)
        {
            order.StartDate = startDate;

            // TODO: Save startdate to the database
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            // Init exceptions
            if (workteam == null)
            {
                throw new ArgumentNullException("A workteam was not given");
            }

            if (orders.Keys.Any(o => orderNumber != null && o.OrderNumber == orderNumber))
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

            workteam.orders.Add(order);

            Connector.CreateOrder(orders, order, workteams[workteam]);

            return order;
        }

        public void SetStartDateOnOrder(Order order, DateTime startDate)
        {
            order.StartDate = startDate;

            // TODO: Add update to database
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

            Assignment assignment = new Assignment();
            assignment.Workform = workform ?? assignment.Workform;
            assignment.Duration = duration ?? assignment.Duration;

            order.assignments.Add(assignment);

            Connector.CreateAssignment(assignments, assignment, orders[order]);

            return assignment;
        }

        public void EditOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            if (orders.Keys.Any(o => o.OrderNumber == orderNumber && o != order))
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

        public Workteam GetWorkteamByName(string foreman)
        {
            List<Workteam> workteams = this.workteams.Keys.ToList();

            Workteam workteam = workteams.Find(o => o.Foreman == foreman);

            if (workteam == null)
            {
                Connector.GetAllWorkteams(this.workteams);

                workteams = this.workteams.Keys.ToList();

                workteam = workteams.Find(o => o.Foreman == foreman);

                if (workteam == null)
                {
                    throw new NotFoundException("The workteam you're trying to get does not exist");
                }
            }

            return workteam;
        }

        public List<Order> GetAllOrdersByWorkteam(Workteam workteam)
        {
            Connector.FillWorkteamWithOrders(orders, workteam, workteams[workteam]);

            return workteam.orders;
        }

        public Workteam CreateWorkteam(string foreman)
        {
            Connector.GetAllWorkteams(workteams);

            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            Workteam workteam = new Workteam(foreman);

            Connector.CreateWorkteam(workteams, workteam);

            return workteam;
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

        private void FillOrderWithAssignments(Order order)
        {
            // Connect with database to get list of assignments, add them to the order assigned.
            int orderId = orders[order];
        }

        public List<Workteam> GetAllWorkteams()
        {
            Connector.GetAllWorkteams(workteams);
            return workteams.Keys.ToList();
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

            for (int i = orderIndex + 1; i < orders.Count; i++)
            {
                Order order = orders[i];

                if (order.StartDate != null) // Is it assigned to the board?
                {
                    SetStartDateOnOrder(order, workteam.GetNextAvailableDate(order));
                }
            }
        }

        public List<Order> ListOfOrdersFromDate(Workteam workteam, DateTime date)
        {
            //loo needs to be sorted 
            List<Order> loo = new List<Order>();
            foreach (Order i in GetAllOrdersByWorkteam(workteam))
            {
                if (i.LastDay(workteam) >= date)
                {
                    loo.Add(i);
                }
            }
            Order temp = new Order();

            for (int write = 0; write < loo.Count; write++)
            {
                for (int sort = 0; sort < loo.Count - 1; sort++)
                {
                    if (loo[sort].StartDate > loo[sort + 1].StartDate)
                    {
                        temp = loo[sort + 1];
                        loo[sort + 1] = loo[sort];
                        loo[sort] = temp;
                    }
                }
            }

            return loo;
            
        }
        

        public void EditForeman(string foremanName, Workteam workteam)
        {

            if (workteams.Where(o => o.Key.Foreman == foremanName).Count() != 0)
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
            return workteams.Remove(workteam);
        }

        public void DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            Offday offday = workteam.GetOffday(date);

            if (workteam.offdays.Remove(offday))
            {
                Connector.DeleteOffday(offdays, offday);
            }
            else
            {
                throw new OffdayNotFoundException();
            }
        }
    }
}
