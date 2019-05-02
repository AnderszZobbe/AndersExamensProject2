using Application_layer;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private ObservableCollection<Order> orders { get; set; }
        private Workteam workteam;
        private Controller controller = Controller.Instance;

        public WorkteamOverview(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Title = $"{workteam.Foreman} - {Title}";

            UpdateOrders();
            GenerateDays();
        }

        private void GenerateDays(int weeks = 5)
        {
            Grid gridDays = OrderGrid.Children.OfType<Grid>().First();
            gridDays.Children.Clear();
            gridDays.ColumnDefinitions.Clear();
            gridDays.RowDefinitions.Clear();

            DateTime dateRoller = DateTime.Now;

            for (int i = 0; i < 2; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                gridDays.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < weeks; i++)
            {

                // Gets the Calendar instance associated with a CultureInfo.
                CultureInfo myCI = new CultureInfo("da-DK");
                System.Globalization.Calendar myCal = myCI.Calendar;

                // Gets the DTFI properties required by GetWeekOfYear.
                CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

                int currentWeek = myCal.GetWeekOfYear(dateRoller, myCWR, myFirstDOW);

                // Border
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Gray,
                    Background = Brushes.LightGray,
                };
                gridDays.Children.Add(border);
                Grid.SetColumn(border, i * 7);
                Grid.SetColumnSpan(border, 7);

                // label
                Label label = new Label()
                {
                    Content = $"Uge {currentWeek + i}",
                    Padding = new Thickness(0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                };
                gridDays.Children.Add(label);
                Grid.SetColumn(label, i * 7);
                Grid.SetColumnSpan(label, 7);
            }

            for (int i = 0; i < 7 * weeks; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                gridDays.ColumnDefinitions.Add(columnDefinition);



                // Button
                Button button = new Button()
                {
                    Content = $"{dateRoller.Day}/{dateRoller.Month}",
                    Padding = new Thickness(0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Background = Brushes.White,
                    DataContext = dateRoller,
                };
                gridDays.Children.Add(button);
                Grid.SetRow(button, 1);
                Grid.SetColumn(button, i);

                if (workteam.IsAnOffday(dateRoller))
                {
                    button.Background = Brushes.Red;
                    button.Click += RemoveOffday;
                }
                else
                {
                    button.Click += AddOffday;
                }



                dateRoller = dateRoller.AddDays(1);
            }
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

            UpdateOrders();
        }

        private void AddOffday(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is DateTime datePicked)
                {
                    AddOffday ao = new AddOffday(workteam, datePicked);
                    ao.Owner = this;
                    ao.ShowDialog();
                    GenerateDays();
                }
            }
        }

        private void RemoveOffday(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateOrders()
        {
            orders = new ObservableCollection<Order>();

            foreach (Order order in controller.GetAllOrdersByWorkteam(workteam))
            {
                orders.Add(order);
            }

            OrderList.ItemsSource = orders;
        }
    }
}
