using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for WorkteamOverview.xaml
    /// </summary>
    public partial class WorkteamOverview : Window
    {
        private ObservableCollection<Order> orders { get; set; }
        private Workteam workteam;
        private Controller controller = Controller.Instance;

        public WorkteamOverview(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Title = $"{workteam.Foreman} - {Title}";

            UpdateOrders();
            GenerateDays();
        }

        private void GenerateDays()
        {
            DateTime dateRoller = DateTime.Now;

            for (int i = 0; i < 7*5; i++)
            {
                GridDays.ColumnDefinitions.Add(new ColumnDefinition());

                Label label = new Label()
                {
                    Content = $"{dateRoller.Day}/{dateRoller.Month}",
                };

                GridDays.Children.Add(label);
                Grid.SetColumn(label, i);

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowWorkteam sw = new ShowWorkteam();
            sw.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocumentNewWorkorder dnw = new DocumentNewWorkorder(workteam);
            dnw.Owner = this;
            dnw.ShowDialog();

            UpdateOrders();
        }

        private void UpdateOrders()
        {
            orders = new ObservableCollection<Order>();

            foreach (Order order in controller.GetAllOrdersByWorkteam(workteam))
            {
                orders.Add(order);
            }

            OrderList.ItemsSource = orders;
        }
    }
}
