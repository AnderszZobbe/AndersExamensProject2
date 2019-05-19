using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditWorkteamForeman.xaml
    /// </summary>
    public partial class EditWorkteamForeman : Window
    {
        private Workteam workteam;
        private Controller controller = Controller.Instance;

        public EditWorkteamForeman(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Foreman.Text = workteam.Foreman;
        }

        private void SaveAndExit(object sender, RoutedEventArgs e)
        {
            controller.UpdateWorkteam(workteam, Foreman.Text);
            Close();
        }
    }
}
