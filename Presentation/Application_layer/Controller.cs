using Application_layer.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool DeleteOrder(Order order)
        {
            return orders.Remove(order);
        }

        public Order CreateAndGetOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline)
        {
            Order order = new Order(workteam)
            {
                OrderNumber = orderNumber,
                Address = address,
                Remark = remark,
                Area = area,
                Amount = amount,
                Prescription = prescription,
                Deadline = deadline
            };

            Connector.CreateOrder(orders, order);

            return order;
        }

        public void SetStartDateOnOrder(Order order, DateTime startDate)
        {
            order.StartDate = startDate;

            // TODO: Add update to database
        }

        public void CreateAssignment(Order order, int? duration = null, Workform? workform = null)
        {
            Assignment assignment = new Assignment();
            assignment.workform = workform ?? assignment.workform;
            assignment.duration = duration ?? assignment.duration;

            order.assignments.Add(assignment);

            Connector.CreateAssignment(assignments, assignment);
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
            Connector.GetAllOrdersByWorkteam(this.orders, workteam);

            List<Order> orders = this.orders.Keys.ToList();

            orders = orders.FindAll(o => o.Workteam == workteam);

            return orders;
        }

        public void CreateWorkteam(string foreman)
        {
            Connector.GetAllWorkteams(workteams);

            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            Workteam workteam = new Workteam(foreman);

            Connector.CreateWorkteam(workteams, workteam);
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration, Workteam workteam)
        {
            DateTime dateRoller = startDate;
            if (workteam == null)
            {
                throw new ArgumentNullException("You are trying to add offdaýs to a nonexistent workteam");
            }
            if (duration <= 0)
            {
                throw new DateOutOfRangeException("Your are trying add an offdate with a duration of 0 or less");
            }
            if (startDate < DateTime.Today)
            {
                throw new DateOutOfRangeException("You are trying to add a task, which starts in the past");
            }
            if (startDate > DateTime.Today.AddYears(1))
            {
                throw new DateOutOfRangeException("You are trying to add a task, which starts in a year");
            }
            for (int i = 0; i < duration - 1; i++)
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

        public bool CustomerExistsByName(string customerName)
        {
            throw new NotImplementedException();
        }

        public void CreateCustomer(string text)
        {
            throw new NotImplementedException();
        }

        public bool AsphaltWorkExistsByName(string text)
        {
            throw new NotImplementedException();
        }

        public void CreateAsphaltWork(string text)
        {
            throw new NotImplementedException();
        }

        public AsphaltCompany GetAsphaltWorkByName(string text)
        {
            throw new NotImplementedException();
        }

        public bool MachineExistsByName(string text)
        {
            throw new NotImplementedException();
        }

        public Machine GetMachineByName(string text)
        {
            throw new NotImplementedException();
        }

        public void CreateMachine(string text)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerByName(string text)
        {
            throw new NotImplementedException();
        }

        public void Reschedule(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Order> ListOfOrdersFromDate(Workteam workteam, DateTime date)
        {
            //loo needs to be sorted 
            //List<Order> loo = new List<Order>();
            //foreach(Order i in orders)
            //{
            //    foreach (Assignment x in i.assignments)
            //    {
            //            if (i.LastDay(workteam) >= date)
            //            {
            //                loo.Add(i);
            //            }
            //            break;
            //    }
            //}
            //return loo
            throw new NotImplementedException();
        }
        

        public void EditForeman(string foremanName, Workteam workteam)
        {

            if (workteams.Where(o => o.Key.Foreman == foremanName).Count() != 0)
            {
                throw new DuplicateObjectException();
            }
            if (foremanName.Count() == 0) { throw new ArgumentException(); }
            //ToDo add DBconnector method
            workteam.Foreman = foremanName ?? throw new ArgumentNullException();
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            return workteams.Remove(workteam);
        }
    }
}
