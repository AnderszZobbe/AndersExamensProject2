using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IConnector
    {
        Workteam CreateWorkteam(string foreman);
        Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration);
        Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);
        Assignment CreateAssignment(Order order, Workform workform, int duration);
        List<Workteam> GetAllWorkteams();
        List<Order> GetAllOrdersFromWorkteam(Workteam workteam);
        List<Offday> GetAllOffdaysFromWorkteam(Workteam workteam);
        List<Assignment> GetAllAssignmentsFromOrder(Order order);
        void Reschedule(Workteam workteam, Order orderToRescheduleFrom, DateTime startDate);
        bool DeleteAllAssignmentsFromOrder(Order order);
        bool WorkteamExists(Workteam workteam);
        bool OffdayExists(Offday offday);
        bool OrderExists(Order order);
        bool AssignmentExists(Assignment assignment);
        bool DeleteOffdayByDate(Workteam workteam, DateTime date);
        void MoveOrderUp(Workteam workteam, Order firstOrder);
        void MoveOrderDown(Workteam workteam, Order firstOrder);
        void UpdateWorkteamForeman(Workteam workteam, string foreman);
        void UpdateOrderStartDate(Order order, DateTime? startDate);
        void SwapOrdersPriority(Workteam workteam, Order firstOrder, Order secondOrder);
        void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);
        bool DeleteWorkteam(Workteam workteam);
        bool DeleteOffday(Workteam workteam, Offday offday);
        bool DeleteOrder(Workteam workteam, Order order);
        bool DeleteAssignment(Order order, Assignment assignment);
    }
}
