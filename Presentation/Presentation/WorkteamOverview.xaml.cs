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
        private int totalWeeks = 7;
        private int startColumn = 0;
        private int clearRowFrom = 1;
        private DateTime startDate = DateTime.Now.AddDays(-14);
        //private DateTime startDate = DateTime.Now.AddDays(0);

        public WorkteamOverview(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;

            Title = $"{workteam.Foreman} - {Title}";

            InitializeDaysColumns(TotalDays);

            UpdateDataGrid();
        }

        private int TotalDays
        {
            get
            {
                return totalWeeks * 7;
            }
        }

        private void InitializeDaysColumns(int days)
        {
            startColumn = GridTemplate.ColumnDefinitions.Count;
            for (int i = 0; i < days; i++)
            {
                GridTemplate.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private Grid InitializeGridRow()
        {
            Grid grid = new Grid();

            grid.Height = 30;

            foreach (ColumnDefinition columnDefinition in GridTemplate.ColumnDefinitions)
            {
                ColumnDefinition cd = new ColumnDefinition
                {
                    Width = columnDefinition.Width
                };
                grid.ColumnDefinitions.Add(cd);
            }

            OrderStack.Children.Add(grid);

            return grid;
        }

        private void InitializeWeeksGrid(Grid grid, DateTime dateRoller)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo ci = new CultureInfo("da-DK");
            System.Globalization.Calendar cal = ci.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek dow = ci.DateTimeFormat.FirstDayOfWeek;

            Grid weekGrid = null;

            for (int i = 0; i < TotalDays; i++)
            {
                if (weekGrid == null || dateRoller.DayOfWeek == dow)
                {
                    weekGrid = new Grid();
                    grid.Children.Add(weekGrid);
                    Grid.SetColumn(weekGrid, i + startColumn);

                    Border weekBorder = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Blue,
                        Margin = new Thickness(2, 0, 2, 0),
                        VerticalAlignment = VerticalAlignment.Bottom,
                    };
                    weekGrid.Children.Add(weekBorder);

                    Label weekLabel = new Label
                    {
                        Padding = new Thickness(0),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = $"Uge {cal.GetWeekOfYear(dateRoller, cwr, dow)}",
                    };
                    weekGrid.Children.Add(weekLabel);
                }
                else
                {
                    Grid.SetColumnSpan(weekGrid, Grid.GetColumnSpan(weekGrid) + 1);
                }

                // Current day
                DisplayCurrentDay(grid, dateRoller, startColumn + i);

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void InitializeOffdaysGrid(Grid grid, DateTime dateRoller)
        {
            // Data
            FillGridData(grid);

            for (int i = 0; i < TotalDays; i++)
            {
                Button btn = new Button();
                btn.DataContext = dateRoller;
                btn.ToolTip = $"{dateRoller.Day}/{dateRoller.Month}/{dateRoller.Year}";

                grid.Children.Add(btn);
                Grid.SetColumn(btn, i + startColumn);

                switch (dateRoller.Date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        btn.Content = "M";
                        break;
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Thursday:
                        btn.Content = "T";
                        break;
                    case DayOfWeek.Wednesday:
                        btn.Content = "O";
                        break;
                    case DayOfWeek.Friday:
                        btn.Content = "F";
                        break;
                    case DayOfWeek.Saturday:
                        btn.Content = "L";
                        break;
                    case DayOfWeek.Sunday:
                        btn.Content = "S";
                        break;
                    default:
                        btn.Content = "??";
                        break;
                }

                if (workteam.IsAnOffday(dateRoller)) // Is the date an offday?
                {
                    switch (workteam.GetOffday(dateRoller).OffdayReason)
                    {
                        case OffdayReason.Weekend:
                            btn.Background = Brushes.Red;
                            break;
                        case OffdayReason.Fredagsfri:
                        case OffdayReason.Helligdag:
                        default:
                            btn.Background = Brushes.DarkRed;
                            break;
                    }

                    btn.Click += RemoveOffday;
                }
                else
                {
                    btn.BorderThickness = new Thickness(1);
                    btn.BorderBrush = Brushes.LightGray;
                    btn.Background = Brushes.White;

                    btn.Click += AddOffday;
                }

                // Current day
                DisplayCurrentDay(grid, dateRoller, i + startColumn);

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void InitializeOrderGrid(Grid grid, Order order, DateTime dateRoller)
        {
            // Includes data
            FillGridData(grid, order);


            for (int i = 0; i < TotalDays; i++)
            {
                Button btn = new Button();
                btn.DataContext = new KeyValuePair<Order, DateTime>(order, dateRoller);
                btn.Click += Reschedule;
                btn.ToolTip = $"{dateRoller.Day}/{dateRoller.Month}/{dateRoller.Year}";

                grid.Children.Add(btn);
                Grid.SetColumn(btn, i + startColumn);

                if (workteam.IsAnOffday(dateRoller)) // Is the date an offday?
                {
                    switch (workteam.GetOffday(dateRoller).OffdayReason)
                    {
                        case OffdayReason.Weekend:
                            btn.Background = Brushes.Red;
                            break;
                        case OffdayReason.Fredagsfri:
                        case OffdayReason.Helligdag:
                        default:
                            btn.Background = Brushes.DarkRed;
                            break;
                    }
                }
                else if (workteam.IsAWorkday(order, dateRoller)) // Is the date a workday?
                {
                    switch (workteam.GetWorkform(order, dateRoller))
                    {
                        case Workform.Dag:
                            btn.Background = Brushes.Orange;
                            break;
                        case Workform.Nat:
                            btn.Background = Brushes.DarkCyan;
                            break;
                        case Workform.Flytning:
                            btn.Background = Brushes.LightGray;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    btn.BorderThickness = new Thickness(1);
                    btn.BorderBrush = Brushes.LightGray;
                    btn.Background = Brushes.White;
                }

                // Deadline
                if (order.Deadline != null && order.Deadline.Value.Date == dateRoller.Date)
                {
                    Border deadline = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };

                    grid.Children.Add(deadline);
                    Grid.SetColumn(deadline, i + startColumn);
                }

                // Current day
                DisplayCurrentDay(grid, dateRoller, i + startColumn);

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void Reschedule(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is KeyValuePair<Order, DateTime> orderAndDateToReschedule)
                {
                    Console.WriteLine("Womba");
                    Order order = orderAndDateToReschedule.Key;
                    DateTime rescheduleDate = orderAndDateToReschedule.Value;

                    controller.Reschedule(workteam, order, rescheduleDate);

                    UpdateDataGrid();
                }
            }
        }

        private void DisplayCurrentDay(Grid grid, DateTime dateRoller, int column)
        {
            if (dateRoller.Date == DateTime.Today)
            {
                Border deadline = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    IsHitTestVisible = false
                };

                grid.Children.Add(deadline);
                Grid.SetColumn(deadline, column);
            }
        }

        private void FillGridData(Grid grid, Order order = null)
        {
            if (order != null)
            {
                AddSimpleColumnLabel(grid, order.OrderNumber, 1);
                AddSimpleColumnLabel(grid, order.Address, 2);
                AddSimpleColumnLabel(grid, order.Remark, 3);
                AddSimpleColumnLabel(grid, order.Area, 4);
                AddSimpleColumnLabel(grid, order.Amount, 5);
                AddSimpleColumnLabel(grid, order.Prescription, 6);
                AddSimpleColumnLabel(grid, order.Customer, 7);
                AddSimpleColumnLabel(grid, order.Machine, 8);
                AddSimpleColumnLabel(grid, order.AsphaltWork, 9);
            }
            else
            {
                AddSimpleColumnLabel(grid, "Order nummer", 1);
                AddSimpleColumnLabel(grid, "Strækning", 2);
                AddSimpleColumnLabel(grid, "Bemærkning", 3);
                AddSimpleColumnLabel(grid, "m2", 4);
                AddSimpleColumnLabel(grid, "tons", 5);
                AddSimpleColumnLabel(grid, "Recept", 6);
                AddSimpleColumnLabel(grid, "Kunde", 7);
                AddSimpleColumnLabel(grid, "Maskine", 8);
                AddSimpleColumnLabel(grid, "Asfaltværk", 9);
            }
        }

        private void UpdateDataGrid()
        {
            DeleteRows();

            Grid grid = InitializeGridRow();

            InitializeWeeksGrid(grid, startDate);

            grid = InitializeGridRow();

            InitializeOffdaysGrid(grid, startDate);

            foreach (Order order in this.workteam.orders)
            {
                grid = InitializeGridRow();

                InitializeOrderGrid(grid, order, startDate);
            }
        }

        private void DeleteRows()
        {
            OrderStack.Children.RemoveRange(clearRowFrom, OrderStack.Children.Count - clearRowFrom);
        }

        private void AddSimpleColumnLabel(Grid grid, object content, int columnIndex)
        {
            // Border
            Border border = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.LightGray,
            };
            grid.Children.Add(border);
            Grid.SetColumn(border, columnIndex);

            Label label = new Label()
            {
                Content = content,
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            grid.Children.Add(label);
            Grid.SetColumn(label, columnIndex);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowWorkteam sw = new ShowWorkteam();
            sw.Show();
        }

        private void DocumentNewWorkorder(object sender, RoutedEventArgs e)
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
                    AddOffday ao;
                    if (datePicked.DayOfWeek == DayOfWeek.Friday)
                    {
                        ao = new AddOffday(workteam, datePicked, datePicked, OffdayReason.Fredagsfri)
                        {
                            Owner = this
                        };
                    }
                    else if (datePicked.DayOfWeek == DayOfWeek.Saturday)
                    {
                        ao = new AddOffday(workteam, datePicked, datePicked.AddDays(1), OffdayReason.Weekend)
                        {
                            Owner = this
                        };
                    }
                    else if (datePicked.DayOfWeek == DayOfWeek.Sunday)
                    {
                        ao = new AddOffday(workteam, datePicked.AddDays(-1), datePicked, OffdayReason.Weekend)
                        {
                            Owner = this
                        };
                    }
                    else
                    {
                        ao = new AddOffday(workteam, datePicked, datePicked, OffdayReason.Helligdag)
                        {
                            Owner = this
                        };
                    }

                    ao.ShowDialog();
                    UpdateDataGrid();
                }
            }
        }

        private void RemoveOffday(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is DateTime datePicked)
                {
                    MessageBoxResult result = MessageBox.Show("Vil du fjerne denne fridags periode?", "Fjern fridags peroide", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            controller.DeleteOffdayByDate(workteam, datePicked);
                            UpdateDataGrid();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
        }
    }
}
