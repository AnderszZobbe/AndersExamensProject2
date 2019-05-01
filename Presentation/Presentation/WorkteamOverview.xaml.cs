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
    /// Interaction logic for WorkteamOverview.xaml
    /// </summary>
    public partial class WorkteamOverview : Window
    {
        private Workteam workteam;

        public WorkteamOverview(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Title = $"{workteam.Foreman} - {Title}";
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
        }
    }
}
