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
            Controller.Connector = new DBConnector();

            Debug();
        }

        private void Debug()
        {
            Controller.Connector = new DBTestConnector();

            Controller controller = Controller.Instance;

            controller.CreateWorkteam("testunit");

            Workteam workteam = controller.GetWorkteamByName("testunit");

            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today.AddDays(3), 0, workteam); // Friday

            controller.CreateOffday(OffdayReason.Weekend, DateTime.Today.AddDays(4), 1, workteam); // Weekend

            controller.CreateOffday(OffdayReason.Holiday, DateTime.Today.AddDays(14), 6, workteam); // Week holiday

            Order order = controller.CreateAndGetOrder(workteam, 123, null, null, null, null, null, null);

            controller.CreateAssignment(order, 4);

            controller.CreateAssignment(order, 1, Workform.Night);

            controller.CreateAssignment(order, 0, Workform.Move);

            controller.SetStartDateOnOrder(order, DateTime.Now.AddDays(7));
            //controller.SetStartDateOnOrder(order, DateTime.Now);

            Order order2 = controller.CreateAndGetOrder(workteam, 321, null, null, null, null, null, null);
        }
    }
}
