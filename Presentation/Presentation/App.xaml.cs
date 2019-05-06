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

            controller.CreateOffday(OffdayReason.Holiday, DateTime.Now, 0, workteam);

            controller.CreateOffday(OffdayReason.Holiday, DateTime.Now.AddDays(3), 2, workteam);

            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Now.AddDays(7), 2, workteam);

            controller.CreateOffday(OffdayReason.Weekend, DateTime.Now.AddDays(13), 2, workteam);

            Order order = controller.CreateAndGetOrder(workteam, 123, "", "", null, null, "", null);

            controller.CreateAssignment(order, 5);

            controller.CreateAssignment(order, 2, Workform.Night);

            controller.SetStartDateOnOrder(order, DateTime.Now.AddDays(10));
        }
    }
}
