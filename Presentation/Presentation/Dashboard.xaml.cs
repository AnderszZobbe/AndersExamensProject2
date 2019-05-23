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
        private ObservableCollection<Workteam> Workteams { get; set; } = new ObservableCollection<Workteam>();
        public Palette PaletteWindow;

        public Dashboard()
        {
            InitializeComponent();

            UpdateWorkteams();

            WorkteamList.ItemsSource = Workteams;
        }

        public void UpdateWorkteams()
        {
            Workteams.Clear();

            foreach (Workteam workteam in controller.GetAllWorkteams())
            {
                Workteams.Add(workteam);
            }
        }

        private void CreateNewWorkteam(object sender, RoutedEventArgs e)
        {
            CreateNewWorkteam cnw = new CreateNewWorkteam
            {
                Owner = this
            };
            cnw.ShowDialog();
            UpdateWorkteams();
        }

        public void UpdateContainer()
        {
            // TODO: Fix it not having to clear all the time
            Container.Children.Clear();

            System.Collections.IList selectedItems = WorkteamList.SelectedItems;

            foreach (object item in selectedItems)
            {
                if (item is Workteam workteam)
                {
                    Frame frame = new Frame
                    {
                        Content = new WorkteamPage(workteam)
                    };

                    Container.Children.Add(frame);
                }
            }
        }

        private void WorkteamSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateContainer();
        }

        private void OpenPalette(object sender, RoutedEventArgs e)
        {
            if (PaletteWindow != null)
            {
                PaletteWindow.Close();
            }

            Palette p = new Palette
            {
                Owner = this
            };

            PaletteWindow = p;
            p.Show();
        }
    }
}
