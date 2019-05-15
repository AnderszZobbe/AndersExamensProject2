using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class Repository
    {
        private IConnector connector;

        public Repository(IConnector connector)
        {
            this.connector = connector ?? throw new ArgumentNullException(nameof(connector));
        }
    }
}
