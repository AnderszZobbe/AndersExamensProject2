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
    public partial class MainWindow : Window
    {
        private ObservableCollection<Workteam> workteams { get; set; } = new ObservableCollection<Workteam>();

        private Controller controller = Controller.Instance;

        public MainWindow()
        {
            InitializeComponent();

            List<Workteam> s = controller.GetAllWorkteams();

            WorkteamList.ItemsSource = workteams;
        }
    }
}
