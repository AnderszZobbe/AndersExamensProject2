using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal interface IWorkteamConnector
    {
        KeyValuePair<Workteam, int> CreateWorkteam(string foreman);

        Dictionary<Workteam, int> GetAllWorkteams();

        void UpdateWorkteamForeman(int workteam, string foreman);

        void DeleteWorkteam(int workteam);
    }
}
