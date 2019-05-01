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
    /// Interaction logic for DocumentNewWorkorder.xaml
    /// </summary>
    public partial class DocumentNewWorkorder : Window
    {
        private Controller controller = Controller.Instance;
        private Workteam workteam;

        public DocumentNewWorkorder(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;
        }

        private void AttemptCreate(object sender, RoutedEventArgs e)
        {
            controller.SaveOrder();
        }
    }
}
