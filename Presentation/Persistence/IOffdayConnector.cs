using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IOffdayConnector
    {
        KeyValuePair<Offday, int> CreateOffday(int workteam, OffdayReason reason, DateTime startDate, int duration);

        Dictionary<Offday, int> GetAllOffdaysByWorkteam(int workteam);



        void DeleteOffday(int offday);
    }
}
