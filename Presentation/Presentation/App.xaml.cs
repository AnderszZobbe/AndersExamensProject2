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

            controller.CreateOrder(workteam, 200, null, null, null, null, null, null);
        }
    }
}
