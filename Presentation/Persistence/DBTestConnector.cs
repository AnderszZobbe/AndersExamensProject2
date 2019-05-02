using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DBTestConnector : IConnector
    {
        private int orderID = 1;
        private int workteamID = 1;

        public void CreateOrder(Dictionary<Order, int> orders, Order order)
        {
            orders.Add(order, orderID++);
        }

        public void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam)
        {
            workteams.Add(workteam, workteamID++);
        }

        public void GetAllOrdersByWorkteam(Dictionary<Order, int> orders, Workteam workteam)
        {
        }

        public void GetAllWorkteams(Dictionary<Workteam, int> workteams)
        {
        }
    }
}
