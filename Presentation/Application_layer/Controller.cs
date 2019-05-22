using Domain.Exceptions;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class Controller : IConnector
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

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            return Connector.CreateOrder(workteam, orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);
        }

        public void UpdateOrderStartDate(Order order, DateTime? startDate)
        {
            Connector.UpdateOrderStartDate(order, startDate);
        }

        public Assignment CreateAssignment(Order order, Workform workform, int duration)
        {
            return Connector.CreateAssignment(order, workform, duration);
        }

        public void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            Connector.UpdateOrder(order, orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);
        }

        public Workteam CreateWorkteam(string foreman)
        {
            return Connector.CreateWorkteam(foreman);
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            return Connector.CreateOffday(workteam, reason, startDate, duration);
        }

        public List<Workteam> GetAllWorkteams()
        {
            return Connector.GetAllWorkteams();
        }

        public void Reschedule(Workteam workteam, Order orderToRescheduleFrom, DateTime startDate)
        {
            Connector.Reschedule(workteam, orderToRescheduleFrom, startDate);
        }

        public void UpdateWorkteamForeman(Workteam workteam, string foreman)
        {
            Connector.UpdateWorkteamForeman(workteam, foreman);
        }

        public bool WorkteamExists(Workteam workteam)
        {
            return Connector.WorkteamExists(workteam);
        }

        public bool OffdayExists(Offday offday)
        {
            return Connector.OffdayExists(offday);
        }

        public bool OrderExists(Order order)
        {
            return Connector.OrderExists(order);
        }

        public bool AssignmentExists(Assignment assignment)
        {
            return Connector.AssignmentExists(assignment);
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
            return Connector.DeleteAllAssignmentsFromOrder(order);
        }

        public bool DeleteOffdayByDate(Workteam workteam, DateTime date)
        {
            return Connector.DeleteOffdayByDate(workteam, date);
        }

        public void MoveOrderUp(Workteam workteam, Order firstOrder)
        {
            Connector.MoveOrderUp(workteam, firstOrder);
        }

        public void MoveOrderDown(Workteam workteam, Order firstOrder)
        {
            Connector.MoveOrderDown(workteam, firstOrder);
        }

        public List<Order> GetAllOrdersFromWorkteam(Workteam workteam)
        {
            return Connector.GetAllOrdersFromWorkteam(workteam);
        }

        public List<Offday> GetAllOffdaysFromWorkteam(Workteam workteam)
        {
            return Connector.GetAllOffdaysFromWorkteam(workteam);
        }

        public List<Assignment> GetAllAssignmentsFromOrder(Order order)
        {
            return Connector.GetAllAssignmentsFromOrder(order);
        }

        public void SwapOrdersPriority(Workteam workteam, Order firstOrder, Order secondOrder)
        {
            Connector.SwapOrdersPriority(workteam, firstOrder, secondOrder);
        }
    }
}
