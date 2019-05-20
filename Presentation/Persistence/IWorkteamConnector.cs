using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IWorkteamConnector
    {
        KeyValuePair<Workteam, int> CreateWorkteam(IConnector connector, string foreman);

        Dictionary<Workteam, int> GetAllWorkteams(IConnector connector);

        void UpdateWorkteamForeman(int workteam, string foreman);

        void DeleteWorkteam(int workteam);
    }
}
