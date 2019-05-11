using Persistence;
using Application_layer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Domain;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Controller.Connector = new DBConnector();

            Debug();
        }

        private void Debug()
        {
            Controller.Connector = new DBTestConnector();

            Controller controller = Controller.Instance;

            Workteam workteam = controller.CreateWorkteam("testunit");

            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today.AddDays(3), 0); // Friday

            controller.CreateOffday(workteam, OffdayReason.Weekend, DateTime.Today.AddDays(4), 1); // Weekend

            controller.CreateOffday(workteam, OffdayReason.Helligdag, DateTime.Today.AddDays(14), 6); // Week holiday

            Order order = controller.CreateOrder(workteam, 123, null, null, null, null, null, null, null, null, null, null);

            controller.CreateAssignment(order, 4, Workform.Dag);

            controller.CreateAssignment(order, 1, Workform.Nat);

            controller.CreateAssignment(order, 0, Workform.Flytning);

            controller.SetStartDateOnOrder(order, DateTime.Now.AddDays(7));
            //controller.SetStartDateOnOrder(order, DateTime.Now);

            Order order2 = controller.CreateOrder(workteam, 321, null, null, null, null, null, DateTime.Now.AddDays(7 * 4), null, null, null, null);
        }
    }
}
