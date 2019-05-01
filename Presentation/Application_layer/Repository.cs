using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    internal class Repository<T>
    {
        private readonly Dictionary<T, int> pairs = new Dictionary<T, int>();

        internal void Create()
        {
            throw new NotImplementedException();
        }

        internal void Exists(T generic)
        {
            throw new NotImplementedException();
        }
    }
}
