using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public interface IConnector
    {
        // Create
        Workteam CreateWorkteam(string foreman);
        Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration);
        Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);
        Assignment CreateAssignment(Order order, Workform workform, int duration);

        // Read
        // > Get All
        List<Workteam> GetAllWorkteams();
        List<Offday> GetAllOffdays();
        List<Order> GetAllOrders();
        List<Assignment> GetAllAssignments();

        // > Exists
        bool WorkteamExists(Workteam workteam);
        bool OffdayExists(Offday offday);
        bool OrderExists(Order order);
        bool AssignmentExists(Assignment assignment);

        // > Fill
        void FillWorkteamWithOrders(Workteam workteam);
        void FillWorkteamWithOffdays(Workteam workteam);
        void FillOrderWithAssignments(Order order);

        // Update
        void UpdateWorkteam(Workteam workteam, string foreman);
        void UpdateOrderStartDate(Order order, DateTime? startDate);
        void SwitchOrdersPriority(Order firstOrder, Order secondOrder);
        void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);

        // Delete
        bool DeleteWorkteam(Workteam workteam);
        bool DeleteOffday(Workteam workteam, Offday offday);
        bool DeleteOrder(Workteam workteam, Order order);
        bool DeleteAssignment(Order order, Assignment assignment);
    }
}
