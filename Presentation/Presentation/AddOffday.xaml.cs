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
    /// Interaction logic for AddOffday.xaml
    /// </summary>
    public partial class AddOffday : Window
    {
        private Controller controller = Controller.Instance;
        private Workteam workteam;

        public AddOffday(Workteam workteam, DateTime startDate, DateTime endDate, OffdayReason offdayReason)
        {
            InitializeComponent();

            this.workteam = workteam;

            StartDate.SelectedDate = startDate;

            EndDate.SelectedDate = endDate;

            ReasonComboBox.ItemsSource = Enum.GetValues(typeof(OffdayReason)).Cast<OffdayReason>();
            ReasonComboBox.SelectedItem = offdayReason;

            SaveButton.Focus();
        }

        private void SaveOffday(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate == null)
            {
                MessageBox.Show("Start dato må ikke være tom.");
                return;
            }

            if (EndDate.SelectedDate == null)
            {
                MessageBox.Show("Slut dato må ikke være tom.");
                return;
            }

            if (StartDate.SelectedDate > EndDate.SelectedDate)
            {
                MessageBox.Show("Slut dato må ikke være før start dato.");
                return;
            }

            try
            {
                int length = ((TimeSpan)(EndDate.SelectedDate - StartDate.SelectedDate)).Days;
                controller.CreateOffday(workteam, (OffdayReason)ReasonComboBox.SelectedItem, StartDate.SelectedDate ?? throw new ArgumentNullException(), length);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            Close();
        }
    }
}
