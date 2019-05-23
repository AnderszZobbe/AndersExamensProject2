using Application_layer;
using Domain;
using Microsoft.Win32;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Presentation.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for WorkteamOverview.xaml
    /// </summary>
    public partial class WorkteamOverview : Window
    {
        private readonly Workteam workteam;
        private readonly Controller controller = Controller.Instance;
        private int totalWeeks = 7;
        private int startColumn = 0;
        private readonly int clearRowFrom = 1;
        private DateTime startDate = DateTime.Today.AddDays(-14);
        private readonly DayOfWeek startDayOfWeek = DayOfWeek.Monday;
        //private DateTime startDate = DateTime.Now.AddDays(0);
        public System.Drawing.Color[] OffdayBrushes = { Settings.Default.Weekend, Settings.Default.FridayFree, Settings.Default.Holiday };
        public System.Drawing.Color[] WorkformBrushes = { Settings.Default.Workday, Settings.Default.Worknight };
        public Palette palette;

        public WorkteamOverview(Workteam workteam)
        {
            // Setback if day is in the middle of a week
            while (startDate.DayOfWeek != startDayOfWeek)
            {
                startDate = startDate.AddDays(-1);
            }

            InitializeComponent();

            this.workteam = workteam;

            SetTitle();

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
            Grid grid = new Grid
            {
                Height = 30
            };

            foreach (ColumnDefinition columnDefinition in GridTemplate.ColumnDefinitions)
            {
                ColumnDefinition cd = new ColumnDefinition
                {
                    Width = columnDefinition.Width,
                    MinWidth = 20,
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
            controller.GetAllOffdaysFromWorkteam(workteam);
            // Data
            FillGridData(grid);

            for (int i = 0; i < TotalDays; i++)
            {
                Button btn = new Button
                {
                    DataContext = dateRoller,
                    ToolTip = $"{dateRoller.Day}/{dateRoller.Month}/{dateRoller.Year}"
                };

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
                    System.Drawing.Color color = OffdayBrushes[(int)workteam.GetOffday(dateRoller).OffdayReason];

                    btn.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
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
            controller.GetAllAssignmentsFromOrder(order);

            // Includes data
            FillGridData(grid, order);


            for (int i = 0; i < TotalDays; i++)
            {
                Button btn = new Button
                {
                    DataContext = new KeyValuePair<Order, DateTime>(order, dateRoller)
                };
                btn.Click += Reschedule;
                btn.ToolTip = $"{dateRoller.Day}/{dateRoller.Month}/{dateRoller.Year}";

                grid.Children.Add(btn);
                Grid.SetColumn(btn, i + startColumn);

                if (workteam.IsAnOffday(dateRoller)) // Is the date an offday?
                {
                    System.Drawing.Color color = OffdayBrushes[(int)workteam.GetOffday(dateRoller).OffdayReason];

                    btn.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                }
                else if (workteam.IsAWorkday(order, dateRoller)) // Is the date a workday?
                {
                    System.Drawing.Color color = WorkformBrushes[(int)workteam.GetWorkform(order, dateRoller)];

                    btn.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
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
                        BorderThickness = new Thickness(2),
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

        private void InitializeNewOrderButton()
        {
            Grid localGrid = InitializeGridRow();

            Button btn = new Button
            {
                Content = "Dokumentér ny arbejdsordre",
                FontSize = 14,
            };
            btn.Click += DocumentNewWorkorder;

            localGrid.Children.Add(btn);
            Grid.SetColumn(btn, 0);
            Grid.SetColumnSpan(btn, startColumn);
        }

        private void Reschedule(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is KeyValuePair<Order, DateTime> orderAndDateToReschedule)
                {
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
                // enable button
                CheckBox cb = new CheckBox
                {
                    DataContext = order
                };
                cb.IsChecked = order.StartDate != null;
                cb.Click += ToggleEnable;
                grid.Children.Add(cb);
                Grid.SetColumn(cb, 0);

                // Priority buttons
                Grid element = new Grid();

                for (int i = 0; i < 2; i++)
                {
                    RowDefinition rd = new RowDefinition
                    {
                        Height = new GridLength(50, GridUnitType.Star)
                    };
                    element.RowDefinitions.Add(rd);
                }

                grid.Children.Add(element);
                Grid.SetColumn(element, 1);

                Button up = new Button
                {
                    Content = "▲",
                    DataContext = order,
                };
                up.Click += MoveOrderUp;
                element.Children.Add(up);

                Button down = new Button
                {
                    Content = "▼",
                    DataContext = order,
                };
                down.Click += MoveOrderDown;
                element.Children.Add(down);
                Grid.SetRow(down, 1);

                // Simple stuff
                AddSimpleColumnLabel(grid, null, 0);
                AddSimpleColumnButton(grid, order.OrderNumber, 2, order);
                AddSimpleColumnButton(grid, order.Address, 3, order);
                AddSimpleColumnButton(grid, order.Remark, 4, order);
                AddSimpleColumnButton(grid, order.Area, 5, order);
                AddSimpleColumnButton(grid, order.Amount, 6, order);
                AddSimpleColumnButton(grid, order.Prescription, 7, order);
                AddSimpleColumnButton(grid, order.Customer, 8, order);
                AddSimpleColumnButton(grid, order.Machine, 9, order);
                AddSimpleColumnButton(grid, order.AsphaltWork, 10, order);
            }
            else
            {
                AddSimpleColumnLabel(grid, null, 0);
                AddSimpleColumnLabel(grid, null, 1);
                AddSimpleColumnLabel(grid, "Ordrenummer", 2);
                AddSimpleColumnLabel(grid, "Strækning", 3);
                AddSimpleColumnLabel(grid, "Bemærkning", 4);
                AddSimpleColumnLabel(grid, "m2", 5);
                AddSimpleColumnLabel(grid, "tons", 6);
                AddSimpleColumnLabel(grid, "Recept", 7);
                AddSimpleColumnLabel(grid, "Kunde", 8);
                AddSimpleColumnLabel(grid, "Maskine", 9);
                AddSimpleColumnLabel(grid, "Asfaltværk", 10);
            }
        }

        private void ToggleEnable(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order order)
                {
                    if (order.StartDate != null)
                    {
                        controller.UpdateOrderStartDate(order, null);
                    }
                    else
                    {
                        controller.UpdateOrderStartDate(order, DateTime.Today);
                        //if (workteam.IsThereAHigherPriorityOrderWithAStartDate(order))
                        //{
                        //    controller.SetStartDateOnOrder(order, workteam.GetNextAvailableDate(workteam.GetNextHigherPriorityOrderWithAStartDate(order)));
                        //}
                        //else
                        //{
                        //    controller.SetStartDateOnOrder(order, DateTime.Today);
                        //}
                    }
                    UpdateDataGrid();
                }
            }
        }

        public void UpdateDataGrid()
        {
            OffdayBrushes = new System.Drawing.Color[] { Settings.Default.Weekend, Settings.Default.FridayFree, Settings.Default.Holiday };
            WorkformBrushes = new System.Drawing.Color[] { Settings.Default.Workday, Settings.Default.Worknight };

            DeleteRows();

            Grid grid = InitializeGridRow();

            InitializeWeeksGrid(grid, startDate);

            grid = InitializeGridRow();

            InitializeOffdaysGrid(grid, startDate);

            foreach (Order order in controller.GetAllOrdersFromWorkteam(workteam))
            {
                grid = InitializeGridRow();

                InitializeOrderGrid(grid, order, startDate);
            }

            InitializeNewOrderButton();
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
                IsHitTestVisible = false,
            };
            grid.Children.Add(border);
            Grid.SetColumn(border, columnIndex);

            Label label = new Label()
            {
                Content = content,
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsHitTestVisible = false,
            };
            grid.Children.Add(label);
            Grid.SetColumn(label, columnIndex);
        }

        private void AddSimpleColumnButton(Grid grid, object content, int columnIndex, Order order)
        {
            // Border
            Border border = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.LightGray,
            };
            grid.Children.Add(border);
            Grid.SetColumn(border, columnIndex);

            Button btn = new Button()
            {
                Content = content,
                DataContext = order,
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            btn.Click += EditOrder;
            grid.Children.Add(btn);
            Grid.SetColumn(btn, columnIndex);
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order order)
                {
                    DocumentNewWorkorder dnw = new DocumentNewWorkorder(workteam, order)
                    {
                        Owner = this
                    };
                    dnw.ShowDialog();
                    UpdateDataGrid();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowWorkteam sw = new ShowWorkteam();
            sw.Show();
        }

        private void DocumentNewWorkorder(object sender, RoutedEventArgs e)
        {
            DocumentNewWorkorder dnw = new DocumentNewWorkorder(workteam)
            {
                Owner = this
            };
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

        private void DeleteCurrentWorkteam(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på du vil fjerne dette arbejdshold?", "Fjernelse af arbejdshold", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    controller.DeleteWorkteam(workteam);
                    Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void EditWorkteamForeman(object sender, RoutedEventArgs e)
        {
            EditWorkteamForeman ewf = new EditWorkteamForeman(workteam)
            {
                Owner = this
            };
            ewf.ShowDialog();
            SetTitle();
        }

        private void MoveOrderUp(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order orderToMove)
                {
                    try
                    {
                        controller.MoveOrderUp(workteam, orderToMove);
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                    UpdateDataGrid();
                }
            }
        }

        private void MoveOrderDown(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order orderToMove)
                {
                    try
                    {
                        controller.MoveOrderDown(workteam, orderToMove);
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                    UpdateDataGrid();
                }
            }
        }

        private void SetTitle()
        {
            Title = $"{workteam.Foreman}";
        }

        private void ShowPalette(object sender, RoutedEventArgs e)
        {
            if (palette != null)
            {
                palette.Close();
            }

            Palette p = new Palette();
            palette = p;
            p.Owner = this;
            p.Show();
        }

        private void PrintToPDF(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = $"3 ugers plan {DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.pdf",
                Filter = "Pdf Files|*.pdf"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                int weeks = 3;
                double sizeMultiplier = 0.62;
                string filePath = saveFileDialog.FileName;

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Size = PageSize.A4;
                page.Orientation = PageOrientation.Landscape;

                // Size
                double margin = 20;
                Point workspace = new Point(page.Width - margin * 2, page.Height - margin * 2);
                Point workspaceOffset = new Point(margin, margin);

                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont defaultFont = new XFont("Verdana", 7);
                XFont boldFont = new XFont("Verdana", 7, XFontStyle.Bold);

                double rowHeight = 20;

                // Start drawing of the orders
                List<Order> orders = controller.GetAllOrdersFromWorkteam(workteam);
                orders = orders.FindAll(o => o.StartDate != null);
                orders = orders.FindAll(o => o.StartDate >= DateTime.Today || workteam.IsAWorkday(o, DateTime.Today));
                orders.Insert(0, null);
                orders.Insert(0, null);
                for (int i = 0; i < orders.Count; i++)
                {
                    double x = 0;

                    for (int j = 2; j < startColumn; j++)
                    {
                        double width = GridTemplate.ColumnDefinitions[j].Width.Value * sizeMultiplier;
                        XPen pen = new XPen(XColors.LightGray, 1);
                        XRect rect = new XRect(x + workspaceOffset.X, i * rowHeight + workspaceOffset.X, width, rowHeight);

                        if (i != 0)
                        {
                            gfx.DrawRectangle(pen, rect);
                        }

                        string content = string.Empty;
                        XFont fontToUse = defaultFont;

                        if (i == 1)
                        {
                            fontToUse = boldFont;
                            switch (j)
                            {
                                case 2:
                                    content = "Ordrenummer";
                                    break;
                                case 3:
                                    content = "Strækning";
                                    break;
                                case 4:
                                    content = "Bemærkning";
                                    break;
                                case 5:
                                    content = "m2";
                                    break;
                                case 6:
                                    content = "Tons";
                                    break;
                                case 7:
                                    content = "Recept";
                                    break;
                                case 8:
                                    content = "Kunde";
                                    break;
                                case 9:
                                    content = "Maskine";
                                    break;
                                case 10:
                                    content = "Asfaltværk";
                                    break;
                            }
                        }
                        else if (i != 0)
                        {
                            switch (j)
                            {
                                case 2:
                                    content = orders[i].OrderNumber.ToString();
                                    break;
                                case 3:
                                    content = orders[i].Address;
                                    break;
                                case 4:
                                    content = orders[i].Remark;
                                    break;
                                case 5:
                                    content = orders[i].Area.ToString();
                                    break;
                                case 6:
                                    content = orders[i].Amount.ToString();
                                    break;
                                case 7:
                                    content = orders[i].Prescription;
                                    break;
                                case 8:
                                    content = orders[i].Customer;
                                    break;
                                case 9:
                                    content = orders[i].Machine;
                                    break;
                                case 10:
                                    content = orders[i].AsphaltWork;
                                    break;
                            }
                        }

                        gfx.DrawString(
                            content,
                            fontToUse,
                            XBrushes.Black,
                            rect,
                            XStringFormats.Center);

                        x += width;
                    }

                    double widthLeft = workspace.X - x;

                    DateTime dateRoller = DateTime.Today;

                    if (i == 0)
                    {
                        for (int j = 0; j < weeks * 7; j++)
                        {
                            double width = widthLeft / (weeks * 7);

                            if (dateRoller.DayOfWeek == startDayOfWeek || j == 0)
                            {
                                XPen pen = new XPen(XColors.LightGray, 1);
                                XRect rect;

                                if (j == 0)
                                {
                                    int days = 0;
                                    while (dateRoller.AddDays(days).DayOfWeek != startDayOfWeek)
                                    {
                                        days++;
                                    }
                                    rect = new XRect(x + workspaceOffset.Y, i * rowHeight + workspaceOffset.Y, width * days, rowHeight);
                                }
                                else if (weeks * 7 - j < 7)
                                {
                                    rect = new XRect(x + workspaceOffset.Y, i * rowHeight + workspaceOffset.Y, width * (weeks * 7 - j), rowHeight);
                                }
                                else
                                {
                                    rect = new XRect(x + workspaceOffset.Y, i * rowHeight + workspaceOffset.Y, width * 7, rowHeight);
                                }


                                gfx.DrawRectangle(pen, rect);


                                CultureInfo ci = new CultureInfo("da-DK");
                                System.Globalization.Calendar cal = ci.Calendar;
                                CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
                                DayOfWeek dow = ci.DateTimeFormat.FirstDayOfWeek;

                                gfx.DrawString(
                                    $"Uge {cal.GetWeekOfYear(dateRoller, cwr, dow)}",
                                    defaultFont,
                                    XBrushes.Black,
                                    rect,
                                    XStringFormats.Center);
                            }


                            x += width;

                            dateRoller = dateRoller.AddDays(1);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < weeks * 7; j++)
                        {
                            double width = widthLeft / (weeks * 7);
                            XPen pen = new XPen(XColors.LightGray, 1);
                            XRect rect = new XRect(x + workspaceOffset.Y, i * rowHeight + workspaceOffset.Y, width, rowHeight);
                            XBrush brush = XBrushes.Transparent;

                            if (workteam.IsAnOffday(dateRoller))
                            {
                                System.Drawing.Color color = OffdayBrushes[(int)workteam.GetOffday(dateRoller).OffdayReason];

                                brush = new XSolidBrush(XColor.FromArgb(color.A, color.R, color.G, color.B));
                            }
                            else if (orders[i] != null && workteam.IsAWorkday(orders[i], dateRoller))
                            {
                                System.Drawing.Color color = WorkformBrushes[(int)workteam.GetWorkform(orders[i], dateRoller)];

                                brush = new XSolidBrush(XColor.FromArgb(color.A, color.R, color.G, color.B));
                            }

                            gfx.DrawRectangle(pen, brush, rect);

                            if (i == 1)
                            {
                                string content = string.Empty;

                                switch (dateRoller.DayOfWeek)
                                {
                                    case DayOfWeek.Sunday:
                                        content = "S";
                                        break;
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
                                }

                                gfx.DrawString(
                                    content,
                                    defaultFont,
                                    XBrushes.Black,
                                    rect,
                                    XStringFormats.Center);
                            }

                            x += width;
                            dateRoller = dateRoller.AddDays(1);
                        }
                    }
                }

                gfx.DrawString(
                    $"Genererede d. {DateTime.Now.ToString()}",
                    defaultFont,
                    XBrushes.Black,
                    new XRect(workspaceOffset.X, workspaceOffset.Y, workspace.X, workspace.Y),
                    XStringFormats.BottomLeft);

                document.Save(filePath);
                Process.Start(filePath);
            }
        }
    }
}
