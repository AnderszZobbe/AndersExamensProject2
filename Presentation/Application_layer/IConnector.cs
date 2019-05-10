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
        void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam);
        void CreateOffday(Dictionary<Offday, int> offdays, Offday offday, int workteamID);
        void CreateOrder(Dictionary<Order, int> orders, Order order, int workteamID);
        void CreateAssignment(Dictionary<Assignment, int> assignments, Assignment assignment, int orderID);

        // Read
        void GetAllWorkteams(Dictionary<Workteam, int> workteams);
        void FillWorkteamWithOrders(Dictionary<Order, int> orders, Workteam workteam, int workteamID);
        void FillWorkteamWithOffdays(Dictionary<Offday, int> offdays, Workteam workteam, int workteamID);
        void FillOrderWithAssignments(Dictionary<Assignment, int> assignments, Order order, int orderID);

        // Update
        void UpdateOrderStartDate(Order order, DateTime startDate);

        // Delete
        void DeleteOffday(Dictionary<Offday, int> offdays, Offday offday);
        void DeleteOrder(Dictionary<Order, int> orders, Workteam workteam, Order order);
    }
}
