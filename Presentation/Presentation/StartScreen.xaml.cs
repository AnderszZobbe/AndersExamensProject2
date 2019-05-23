using Application_layer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            InitializeComponent();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                TestConnection();
            }).Start();
        }

        public void TestConnection()
        {
            Thread.Sleep(500);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadingLabel.Content = "Testing connection...";
                LoadingBar.Value = 20;
            }));

            try
            {
                Controller.Instance.GetAllWorkteams();
            }
            catch (SqlException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til serveren, tjek dit internet og prøv igen.", "No connection");

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Close();
                }));

                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadingLabel.Content = "Finishing up...";
                LoadingBar.Value = 100;
            }));

            Thread.Sleep(500);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                Close();
            }));
        }
    }
}
