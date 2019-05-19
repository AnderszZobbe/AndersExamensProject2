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
        public static readonly List<DataWorkteam> workteams = new List<DataWorkteam>();

        private int id;
        private IDataProvider dataProvider;

        public DataWorkteam(IDataProvider dataProvider, int id, string foreman) : base(foreman)
        {
            this.dataProvider = dataProvider;
            this.id = id;
            workteams.Add(this);
        }

        public override string Foreman
        {
            get => base.Foreman;
            set
            {
                if (workteams.Any(o => o != this && o.Foreman == value))
                {
                    throw new ArgumentException("Another workteam already exists with that foreman");
                }

                string oldForeman = Foreman;
                base.Foreman = value;

                if (dataProvider != null)
                {
                    try
                    {
                        dataProvider.UpdateWorkteamForeman(id, Foreman);
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
