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
    /// Interaction logic for CreateNewWorkteam.xaml
    /// </summary>
    public partial class CreateNewWorkteam : Window
    {
        internal Workteam Workteam;

        public CreateNewWorkteam()
        {
            InitializeComponent();
        }

        private void SaveNewWorkteam(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (foreman.Text == string.Empty)
            {
                error = true;
            }

            if (!error)
            {
                Workteam = new Workteam();

                Close();
            }
        }
    }
}
