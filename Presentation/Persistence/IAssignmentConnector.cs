using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IAssignmentConnector
    {
        KeyValuePair<Assignment, int> CreateAssignment(int order, Workform workform, int duration);
        Dictionary<Assignment, int> GetAllAssignmentsByOrder(int order);
        void DeleteAssignment(int assignment);
    }
}
