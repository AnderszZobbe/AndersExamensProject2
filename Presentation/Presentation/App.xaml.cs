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

            controller.CreateAndGetOrder(workteam, 200, null, null, null, null, null, null);

            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today.AddDays(3), 0, workteam); // Friday

            controller.CreateOffday(OffdayReason.Weekend, DateTime.Today.AddDays(4), 1, workteam); // Weekend

            controller.CreateOffday(OffdayReason.Holiday, DateTime.Today.AddDays(14), 6, workteam); // Week holiday

            Order order = controller.CreateAndGetOrder(workteam, 123, "", "", null, null, "", null);

            controller.CreateAssignment(order, 5);

            controller.CreateAssignment(order, 2, Workform.Night);

            controller.SetStartDateOnOrder(order, DateTime.Now.AddDays(10));
        }
    }
}
