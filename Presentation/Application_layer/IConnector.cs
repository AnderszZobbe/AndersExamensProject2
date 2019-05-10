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
        Offday CreateOffday(Offday offday, Workteam workteam);
        Order CreateOrder(Order order, Workteam workteam);
        Assignment CreateAssignment(Assignment assignment, Order order);

        // Read
        // > Get All
        List<Workteam> GetAllWorkteams();
        List<Order> GetAllOrders();
        List<Offday> GetAllOffdays();
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
        void UpdateWorkteamForeman(Workteam workteam, string foreman);
        void UpdateOrderStartDate(Order order, DateTime startDate);

        // Delete
        bool DeleteOffday(Offday offday, Workteam workteam);
        bool DeleteOrder(Workteam workteam, Order order);
        bool DeleteWorkteam(Workteam workteam);
        bool DeleteAssignment(Order order, Assignment assignment);
    }
}
