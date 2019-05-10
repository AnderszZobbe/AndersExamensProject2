using Application_layer.DataClasses;
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
        WorkteamData CreateWorkteam(string foreman);
        OffdayData CreateOffday(OffdayData offday, WorkteamData workteam);
        OrderData CreateOrder(OrderData order, WorkteamData workteam);
        AssignmentData CreateAssignment(AssignmentData assignment, OrderData order);

        // Read
        List<WorkteamData> GetAllWorkteams();
        List<OrderData> GetAllOrders();
        List<OffdayData> GetAllOffdays();
        List<AssignmentData> GetAllAssignments();
        void FillWorkteamWithOrders(WorkteamData workteam);
        void FillWorkteamWithOffdays(WorkteamData workteam);
        void FillOrderWithAssignments(OrderData order);

        // Update
        void UpdateOrderStartDate(OrderData order, DateTime startDate);

        // Delete
        bool DeleteOffday(OffdayData offday, WorkteamData workteam);
        bool DeleteOrder(WorkteamData workteam, OrderData order);
        bool DeleteWorkteam(WorkteamData workteam);
        bool DeleteAssignment(OrderData order, AssignmentData assignment);
    }
}
