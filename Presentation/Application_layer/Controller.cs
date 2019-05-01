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
        private readonly DBConnector dBConnector = new DBConnector();

        private readonly Dictionary<Order, int> orders = new Dictionary<Order, int>();
        private readonly Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();

        private Controller()
        {
        }

        public static Controller Instance { get; } = new Controller();

        public bool DeleteOrder(Order order)
        {
            return orders.Remove(order);
        }

        public void CreateOrder(int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, Customer customer, AsphaltCompany asphaltWork, Machine machine)
        {
            Order order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, customer, asphaltWork, machine);

            dBConnector.CreateOrder(orders, order);
        }

        public Workteam GetWorkteamByName(string foreman)
        {
            List<Workteam> workteams = this.workteams.Keys.ToList();

            Workteam workteam = workteams.Find(o => o.Foreman == foreman);

            if (workteam == null)
            {
                dBConnector.GetAllWorkteams(this.workteams);

                workteams = this.workteams.Keys.ToList();

                workteam = workteams.Find(o => o.Foreman == foreman);

                if (workteam == null)
                {
                    throw new NotFoundException("The workteam you're trying to get does not exist");
                }
            }

            return workteam;
        }

        public void CreateWorkteam(string foreman)
        {
            dBConnector.GetAllWorkteams(workteams);

            if (workteams.Keys.Any(o => o.Foreman == foreman))
            {
                throw new DuplicateObjectException("There already exsits a workteam with a foreman by that name");
            }

            Workteam workteam = new Workteam(foreman);

            dBConnector.CreateWorkteam(workteams, workteam);
        }

        public void CreateOffday(OffdayReason reason, DateTime startDate, int duration, Workteam workteam)
        {
            Offday offday = new Offday(reason, startDate, duration);
            //ToDo add DBconnector method
            workteam.offdays.Add(offday);
            //ToDo Reschedule(date);
        }

        public List<Workteam> GetAllWorkteams()
        {
            dBConnector.GetAllWorkteams(workteams);
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
