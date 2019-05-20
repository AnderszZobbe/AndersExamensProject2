using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataClasses
{
    public class DataWorkteam : Workteam
    {
        private int id;
        private IConnector connector;

        public DataWorkteam(IConnector connector, int id, string foreman) : base(foreman)
        {
            this.connector = connector;
            this.id = id;
        }

        public override string Foreman
        {
            get => base.Foreman;
            set
            {
                string oldForeman = Foreman;
                base.Foreman = value;

                if (connector != null)
                {
                    try
                    {
                        connector.UpdateWorkteam(this, Foreman);
                    }
                    catch (Exception)
                    {
                        base.Foreman = oldForeman;
                        throw;
                    }
                }
            }
        }
    }
}
