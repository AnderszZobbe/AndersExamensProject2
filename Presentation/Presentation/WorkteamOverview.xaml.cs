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
        private int totalWeeks = 5;
        private int startColumn = 0;
        private int clearRowFrom = 1;

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
                        case OffdayReason.FridayFree:
                        case OffdayReason.Holiday:
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

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void InitializeOrderGrid(Grid grid, Order order, DateTime dateRoller)
        {
            // Includes data
            FillGridData(grid, order);


            for (int i = 0; i < TotalDays; i++)
            {
                Border btn = new Border();
                btn.DataContext = dateRoller;
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
                        case OffdayReason.FridayFree:
                        case OffdayReason.Holiday:
                        default:
                            btn.Background = Brushes.DarkRed;
                            break;
                    }
                }
                else if (workteam.IsAWorkday(order, dateRoller)) // Is the date a workday?
                {
                    switch (workteam.GetWorkform(order, dateRoller))
                    {
                        case Workform.Day:
                            btn.Background = Brushes.Orange;
                            break;
                        case Workform.Night:
                            btn.Background = Brushes.DarkCyan;
                            break;
                        case Workform.Move:
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

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void FillGridData(Grid grid, Order order = null)
        {
            if (order != null)
            {
                AddSimpleColumnLabel(grid, order.OrderNumber, 1);
                AddSimpleColumnLabel(grid, order.Prescription, 2);
                AddSimpleColumnLabel(grid, order.Remark, 3);
                AddSimpleColumnLabel(grid, order.Priority, 4);
                AddSimpleColumnLabel(grid, order.OrderNumber, 5);
            }
            else
            {
                AddSimpleColumnLabel(grid, "Order nummer", 1);
            }
        }

        private void UpdateDataGrid()
        {
            DeleteRows();

            Grid grid = InitializeGridRow();

            InitializeWeeksGrid(grid, DateTime.Now);

            grid = InitializeGridRow();

            InitializeOffdaysGrid(grid, DateTime.Now);

            foreach (Order order in this.workteam.orders)
            {
                grid = InitializeGridRow();

                InitializeOrderGrid(grid, order, DateTime.Now);
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
                    AddOffday ao = new AddOffday(workteam, datePicked, datePicked)
                    {
                        Owner = this
                    };
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
