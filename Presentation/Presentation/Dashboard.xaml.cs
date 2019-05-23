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
        private bool initializingFinished;
        private Controller controller = Controller.Instance;
        public Palette PaletteWindow;
        private int totalDays = 7 * 7;
        private DateTime startDate = DateTime.Today.AddDays(-14);
        private readonly DayOfWeek startDayOfWeek = DayOfWeek.Monday;

        public Dashboard()
        {
            InitializeComponent();

            // Setback if day is in the middle of a week
            while (startDate.DayOfWeek != startDayOfWeek)
            {
                startDate = startDate.AddDays(-1);
            }

            DatePickerStartDate.SelectedDate = startDate;
            DatePickerEndDate.SelectedDate = startDate.AddDays(totalDays - 1);

            DatePickerStartDate.DisplayDateEnd = DatePickerEndDate.SelectedDate;

            DatePickerEndDate.DisplayDateStart = DatePickerStartDate.SelectedDate;

            initializingFinished = true;

            UpdateWorkteams();

            WorkteamList.ItemsSource = Workteams;
        }
        private ObservableCollection<Workteam> Workteams { get; set; } = new ObservableCollection<Workteam>();

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
                        Content = new WorkteamPage(workteam, startDate, totalDays)
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

        private void DatePickerStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initializingFinished && DatePickerStartDate.SelectedDate.HasValue)
            {
                startDate = DatePickerStartDate.SelectedDate.Value;

                if (initializingFinished && DatePickerEndDate.SelectedDate.HasValue)
                {
                    DateTime endDate = DatePickerEndDate.SelectedDate.Value;
                    totalDays = (int)(endDate - startDate).TotalDays + 1;
                }

                DatePickerStartDate.DisplayDateEnd = DatePickerEndDate.SelectedDate;

                DatePickerEndDate.DisplayDateStart = DatePickerStartDate.SelectedDate;

                UpdateContainer();
            }
        }

        private void DatePickerEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initializingFinished && DatePickerEndDate.SelectedDate.HasValue)
            {
                DateTime endDate = DatePickerEndDate.SelectedDate.Value;
                totalDays = (int)(endDate - startDate).TotalDays + 1;

                DatePickerStartDate.DisplayDateEnd = DatePickerEndDate.SelectedDate;

                DatePickerEndDate.DisplayDateStart = DatePickerStartDate.SelectedDate;

                UpdateContainer();
            }
        }
    }
}
