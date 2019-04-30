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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShowWorkteam : Window
    {
        private ObservableCollection<Workteam> workteams { get; set; } = new ObservableCollection<Workteam>();

        private Controller controller = Controller.Instance;

        public ShowWorkteam()
        {
            InitializeComponent();

            foreach (Workteam workteam in controller.GetAllWorkteams())
            {
                workteams.Add(workteam);
            }

            WorkteamList.ItemsSource = workteams;
        }

        private void CreateNewWorkteam(object sender, RoutedEventArgs e)
        {
            CreateNewWorkteam cnw = new CreateNewWorkteam();
            cnw.Owner = this;
            cnw.ShowDialog();

            if (cnw.Workteam != null)
            {
                WorkteamOverview wo = new WorkteamOverview(cnw.Workteam);

                wo.Show();
                Close();
            }
        }

        private void WorkteamListSelect(object sender, RoutedEventArgs e)
        {
            if (sender is Selector element)
            {
                if (element.SelectedItem is Workteam workteam)
                {
                    OpenWorkteam(workteam);
                }
            }
        }

        private void OpenWorkteam(Workteam workteam)
        {
            WorkteamOverview wo = new WorkteamOverview(workteam);
            wo.Show();
            Close();
        }
    }
}
