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
        void GetAllWorkteams(Dictionary<Workteam, int> workteams);
        void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam);
        void CreateOrder(Dictionary<Order, int> orders, Order order);
        void FillWorkteamWithOrders(Dictionary<Order, int> orders, Workteam workteam);
        void GetAllAssignemntsByOrder(Dictionary<Order, int> orders, Workteam workteam);
        void CreateAssignment(Dictionary<Assignment, int> assignments, Assignment assignment);
    }
}
