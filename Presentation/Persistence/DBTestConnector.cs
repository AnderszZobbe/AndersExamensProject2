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
        public void CreateOrder(Dictionary<Order, int> orders, Order order)
        {
            throw new NotImplementedException();
        }

        public void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public void GetAllWorkteams(Dictionary<Workteam, int> workteams)
        {
            throw new NotImplementedException();
        }
    }
}
