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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private Controller controller = Controller.Instance;
        private ObservableCollection<Workteam> workteams { get; set; } = new ObservableCollection<Workteam>();

        public Dashboard()
        {
            InitializeComponent();
        }

        private void UpdateWorkteams()
        {

            foreach (Workteam workteam in controller.GetAllWorkteams())
            {
                workteams.Add(workteam);
            }

            WorkteamList.ItemsSource = workteams;
        }
    }
}
