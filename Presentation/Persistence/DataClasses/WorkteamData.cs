using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataClasses
{
    public class WorkteamData : Workteam
    {
        private IConnector connector;

        public WorkteamData(IConnector connector, string foreman) : base(foreman)
        {
            this.connector = connector;
        }

        public override string Foreman
        {
            get => base.Foreman;
            set => base.Foreman = value;
        }
    }
}
