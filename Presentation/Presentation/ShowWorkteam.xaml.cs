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

            List<Workteam> s = controller.GetAllWorkteams();

            //workteams = s;

            WorkteamList.ItemsSource = workteams;
        }

        private void CreateNewWorkteam(object sender, RoutedEventArgs e)
        {
            CreateNewWorkteam cnw = new CreateNewWorkteam();
            cnw.Owner = this;
            cnw.ShowDialog();

            if (cnw.Workteam != null)
            {
                controller.CreateWorkteam(cnw.Workteam.Foreman);

                WorkteamOverview wo = new WorkteamOverview();

                wo.Show();
                Close();
            }
        }
    }
}
