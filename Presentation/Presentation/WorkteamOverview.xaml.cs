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
        private Workteam workteam;
        private Controller controller = Controller.Instance;

        public WorkteamOverview(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Title = $"{workteam.Foreman} - {Title}";

            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            OrderStack.Children.RemoveRange(2, OrderStack.Children.Count - 2);
            GenerateGridHeader(5);
            GenerateGridOrders(5);
        }

        private void GenerateGridHeader(int weeks)
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

            int weekEnder = 0;
            int totalDays = 7 * weeks;

            for (int i = 0; i < totalDays; i++)
            {
                if (weekEnder <= 0) // New week has started.
                {

                }



                ColumnDefinition columnDefinition = new ColumnDefinition();
                gridDays.ColumnDefinitions.Add(columnDefinition);


                string content;
                switch (dateRoller.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        content = "M";
                        break;
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Thursday:
                        content = "T";
                        break;
                    case DayOfWeek.Wednesday:
                        content = "O";
                        break;
                    case DayOfWeek.Friday:
                        content = "F";
                        break;
                    case DayOfWeek.Saturday:
                        content = "L";
                        break;
                    case DayOfWeek.Sunday:
                        content = "S";
                        break;
                    default:
                        content = "??";
                        break;
                }

                // Button
                Button button = new Button()
                {
                    Content = content,
                    ToolTip = $"{dateRoller.Day}/{dateRoller.Month}/{dateRoller.Year}",
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

        private void GenerateGridOrders(int weeks)
        {
            foreach (Order order in controller.GetAllOrdersByWorkteam(workteam))
            {
                Grid grid = new Grid()
                {
                    Height = 30,
                };
                OrderStack.Children.Add(grid);

                AddSimpleColumnLabel(grid, "Klar", new GridLength(30));
                AddSimpleColumnLabel(grid, order.OrderNumber, new GridLength(90));
                AddSimpleColumnLabel(grid, order.Address, new GridLength(120));
                AddSimpleColumnLabel(grid, "Maskine", new GridLength(60));
                AddSimpleColumnLabel(grid, order.Area, new GridLength(30));
                AddSimpleColumnLabel(grid, order.Amount, new GridLength(60));
                AddColumnDays(grid, weeks, order);
            }
        }

        private void AddSimpleColumnLabel(Grid grid, object content, GridLength size)
        {
            ColumnDefinition columnDefinition = new ColumnDefinition
            {
                Width = size
            };
            grid.ColumnDefinitions.Add(columnDefinition);

            // Border
            Border border = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.LightGray,
            };
            grid.Children.Add(border);
            Grid.SetColumn(border, grid.ColumnDefinitions.Count - 1);

            Label label = new Label()
            {
                Content = content,
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            grid.Children.Add(label);
            Grid.SetColumn(label, grid.ColumnDefinitions.Count - 1);
        }

        private void AddColumnDays(Grid grid, int weeks, Order order)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star),
            });

            Grid gridDays = new Grid()
            {
                Background = Brushes.Pink,
            };
            grid.Children.Add(gridDays);
            Grid.SetColumn(gridDays, grid.ColumnDefinitions.Count - 1);

            DateTime dateRoller = DateTime.Now;

            int orderLength = order.GetTotalDuration();

            for (int i = 0; i < 7 * weeks; i++)
            {
                gridDays.ColumnDefinitions.Add(new ColumnDefinition());

                // Border
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.LightGray,
                    Background = Brushes.White,
                };
                gridDays.Children.Add(border);
                Grid.SetRow(border, 1);
                Grid.SetColumn(border, i);

                if (workteam.IsAnOffday(dateRoller)) // Is the date an offday?
                {
                    border.BorderThickness = new Thickness();

                    switch (workteam.GetOffdayReason(dateRoller))
                    {
                        case OffdayReason.Weekend:
                            border.Background = Brushes.PaleVioletRed;
                            break;
                        case OffdayReason.FridayFree:
                            border.Background = Brushes.DarkRed;
                            break;
                        case OffdayReason.Holiday:
                        default:
                            border.Background = Brushes.Red;
                            break;
                    }
                }
                else if(workteam.IsAWorkday(order, dateRoller)) // Is the date a workday?
                {
                    border.Background = Brushes.Orange;
                    border.BorderBrush = Brushes.DarkOrange;
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

            UpdateDataGrid();
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
                    UpdateDataGrid();
                }
            }
        }

        private void RemoveOffday(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
