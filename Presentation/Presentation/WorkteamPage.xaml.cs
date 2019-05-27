using Application_layer;
using Domain;
using Microsoft.Win32;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Presentation.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for WorkteamPage.xaml
    /// </summary>
    public partial class WorkteamPage : Page
    {
        private readonly Workteam workteam;
        private readonly Controller controller = Controller.Instance;
        private int totalDays = 7 * 7;
        private DateTime startDate = DateTime.Today.AddDays(-14);
        private readonly DayOfWeek startDayOfWeek = DayOfWeek.Monday;
        public System.Drawing.Color[] OffdayBrushes = { Settings.Default.Weekend, Settings.Default.FridayFree, Settings.Default.Holiday };
        public System.Drawing.Color[] WorkformBrushes = { Settings.Default.Workday, Settings.Default.Worknight };
        private delegate void OnScroll(ScrollViewer scrollViewer);
        private static event OnScroll OnScrollCollection;

        public WorkteamPage(Workteam workteam, DateTime startDate, int totalDays)
        {
            this.startDate = startDate;
            this.totalDays = totalDays;

            InitializeComponent();

            this.workteam = workteam;

            InitializeGrid();

            OnScrollCollection += OnScrollEvent;
        }

        public void InitializeGrid()
        {
            OffdayBrushes = new System.Drawing.Color[] { Settings.Default.Weekend, Settings.Default.FridayFree, Settings.Default.Holiday };
            WorkformBrushes = new System.Drawing.Color[] { Settings.Default.Workday, Settings.Default.Worknight };

            ClearRows();
            InitializeWeekRow();
            InitializeOffdayAndDescriptionRow();
            InitializeOrderRows();
            InitializeNewOrderButton();
        }

        private void ClearRows()
        {
            OrderStack.Children.RemoveRange(1, OrderStack.Children.Count - 1);
        }

        private void InitializeWeekRow()
        {
            Grid gridRow = InitializeNewGridRow();

            // Days
            Grid gridRowDays = InitializeNewDaysGrid(gridRow);

            /* Calendar stuff -- START */
            CultureInfo ci = new CultureInfo("da-DK");
            System.Globalization.Calendar cal = ci.Calendar;
            CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek dow = ci.DateTimeFormat.FirstDayOfWeek;
            /* Calendar stuff -- END */

            DateTime dateRoller = startDate;

            Grid weekGrid = null;
            for (int i = 0; i < totalDays; i++)
            {
                if (i == 0 || dateRoller.DayOfWeek == dow)
                {
                    weekGrid = new Grid();
                    gridRowDays.Children.Add(weekGrid);
                    Grid.SetColumn(weekGrid, i);

                    Border weekBorder = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Blue,
                        Margin = new Thickness(2, 0, 2, 0),
                        VerticalAlignment = VerticalAlignment.Bottom,
                    };
                    weekGrid.Children.Add(weekBorder);
                }
                else
                {
                    Grid.SetColumnSpan(weekGrid, Grid.GetColumnSpan(weekGrid) + 1);
                }

                if (weekGrid.Children.Count <= 1 && Grid.GetColumnSpan(weekGrid) > 2)
                {
                    Label weekLabel = new Label
                    {
                        Padding = new Thickness(0),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = $"Uge {cal.GetWeekOfYear(dateRoller, cwr, dow)}",
                    };
                    weekGrid.Children.Add(weekLabel);
                }

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void InitializeOffdayAndDescriptionRow()
        {
            Grid gridRow = InitializeNewGridRow();

            for (int i = 0; i < gridRow.ColumnDefinitions.Count - 1; i++)
            {
                switch (i)
                {
                    case 2:
                        AddSimpleLabel(gridRow, "Ordrenummer", i);
                        break;
                    case 3:
                        AddSimpleLabel(gridRow, "Strækning", i);
                        break;
                    case 4:
                        AddSimpleLabel(gridRow, "Bemærkning", i);
                        break;
                    case 5:
                        AddSimpleLabel(gridRow, "m2", i);
                        break;
                    case 6:
                        AddSimpleLabel(gridRow, "Tons", i);
                        break;
                    case 7:
                        AddSimpleLabel(gridRow, "Recept", i);
                        break;
                    case 8:
                        AddSimpleLabel(gridRow, "Kunde", i);
                        break;
                    case 9:
                        AddSimpleLabel(gridRow, "Maskine", i);
                        break;
                    case 10:
                        AddSimpleLabel(gridRow, "Asfaltværk", i);
                        break;
                }
            }

            // Days
            controller.GetAllOffdaysFromWorkteam(workteam);

            Grid gridRowDays = InitializeNewDaysGrid(gridRow);

            DateTime dateRoller = startDate;

            for (int i = 0; i < gridRowDays.ColumnDefinitions.Count; i++)
            {
                string content = "?";

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
                    default:
                        break;
                }
                Button button = AddSimpleButton(gridRowDays, content, i);

                button.DataContext = dateRoller;

                if (workteam.IsAnOffday(dateRoller))
                {
                    System.Drawing.Color color = OffdayBrushes[(int)workteam.GetOffday(dateRoller).OffdayReason];

                    button.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));

                    button.Click += RemoveOffday;
                }
                else
                {
                    button.Background = Brushes.Transparent;

                    button.Click += AddOffday;
                }

                if (DateTime.Today == dateRoller.Date)
                {
                    AddLeftLine(gridRowDays, i);
                }

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void InitializeOrderRows()
        {
            foreach (Order order in controller.GetAllOrdersFromWorkteam(workteam))
            {
                InitializeOrderRow(order);
            }
        }

        private void InitializeOrderRow(Order order)
        {
            Grid gridRow = InitializeNewGridRow();

            for (int i = 0; i < gridRow.ColumnDefinitions.Count - 1; i++)
            {
                if (i == 0)
                {
                    AddSimpleLabel(gridRow, null, i);
                    CheckBox cb = new CheckBox
                    {
                        DataContext = order
                    };
                    cb.IsChecked = order.StartDate != null;
                    cb.Click += ToggleEnable;
                    Grid.SetColumn(cb, i);
                    gridRow.Children.Add(cb);
                }
                else if (i == 1)
                {
                    Grid element = new Grid();

                    for (int j = 0; j < 2; j++)
                    {
                        RowDefinition rd = new RowDefinition
                        {
                            Height = new GridLength(50, GridUnitType.Star)
                        };
                        element.RowDefinitions.Add(rd);
                    }

                    gridRow.Children.Add(element);
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
                }
                else
                {
                    string content = string.Empty;

                    switch (i)
                    {
                        case 2:
                            content = order.OrderNumber.ToString();
                            break;
                        case 3:
                            content = order.Address;
                            break;
                        case 4:
                            content = order.Remark;
                            break;
                        case 5:
                            content = order.Area.ToString();
                            break;
                        case 6:
                            content = order.Amount.ToString();
                            break;
                        case 7:
                            content = order.Prescription;
                            break;
                        case 8:
                            content = order.Customer;
                            break;
                        case 9:
                            content = order.Machine;
                            break;
                        case 10:
                            content = order.AsphaltWork;
                            break;
                    }

                    Button button = AddSimpleButton(gridRow, content, i);
                    button.Background = Brushes.Transparent;
                    button.DataContext = order;
                    button.Click += EditOrder;
                }
            }


            // Days
            controller.GetAllAssignmentsFromOrder(order);

            Grid gridRowDays = InitializeNewDaysGrid(gridRow);

            DateTime dateRoller = startDate;

            for (int i = 0; i < gridRowDays.ColumnDefinitions.Count; i++)
            {
                Button button = AddSimpleButton(gridRowDays, null, i);
                button.DataContext = new KeyValuePair<Order, DateTime>(order, dateRoller);
                button.Click += Reschedule;

                if (workteam.IsAnOffday(dateRoller))
                {
                    System.Drawing.Color color = OffdayBrushes[(int)workteam.GetOffday(dateRoller).OffdayReason];

                    button.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                }
                else if (workteam.IsAWorkday(order, dateRoller))
                {
                    System.Drawing.Color color = WorkformBrushes[(int)workteam.GetWorkform(order, dateRoller)];

                    button.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                }
                else
                {
                    button.Background = Brushes.Transparent;
                }

                if (order.Deadline != null && order.Deadline == dateRoller)
                {
                    Border border = new Border()
                    {
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        IsHitTestVisible = false
                    };
                    Grid.SetColumn(border, i);
                    gridRowDays.Children.Add(border);
                }

                if (DateTime.Today == dateRoller.Date)
                {
                    AddLeftLine(gridRowDays, i);
                }

                dateRoller = dateRoller.AddDays(1);
            }
        }

        private void AddLeftLine(Grid grid, int column)
        {
            Border border = new Border()
            {
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.Red,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsHitTestVisible = false
            };
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        private void InitializeNewOrderButton()
        {
            Grid grid = InitializeGridRow();
            Button button = AddSimpleButton(grid, "Dokumentér ny arbejdsordre", 0);
            Grid.SetColumnSpan(button, 11);
            button.Click += DocumentNewWorkorder;
        }

        private Grid InitializeNewGridRow()
        {
            Grid gridRow = new Grid
            {
                Height = 30
            };

            for (int i = 0; i < GridTemplate.ColumnDefinitions.Count; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition
                {
                    Width = GridTemplate.ColumnDefinitions[i].Width
                };

                gridRow.ColumnDefinitions.Add(columnDefinition);
            }

            OrderStack.Children.Add(gridRow);
            return gridRow;
        }

        private Grid InitializeNewDaysGrid(Grid grid)
        {
            Grid gridRowDays = new Grid();

            for (int i = 0; i < totalDays; i++)
            {
                var columnDefinition = new ColumnDefinition
                {
                    MinWidth = 20
                };

                gridRowDays.ColumnDefinitions.Add(columnDefinition);
            }

            Grid.SetColumn(gridRowDays, grid.ColumnDefinitions.Count - 1);
            grid.Children.Add(gridRowDays);
            return gridRowDays;
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

        private void Reschedule(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is KeyValuePair<Order, DateTime> orderAndDateToReschedule)
                {
                    Order order = orderAndDateToReschedule.Key;
                    DateTime rescheduleDate = orderAndDateToReschedule.Value;

                    controller.Reschedule(workteam, order, rescheduleDate);

                    InitializeGrid();
                }
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
                    InitializeGrid();
                }
            }
        }

        private Button AddSimpleButton(Grid grid, object content, int columnIndex)
        {
            // Border
            Button button = new Button()
            {
                Content = content,
                BorderBrush = Brushes.LightGray
            };
            grid.Children.Add(button);
            Grid.SetColumn(button, columnIndex);

            return button;
        }

        private void AddSimpleLabel(Grid grid, object content, int columnIndex)
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

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order order)
                {
                    DocumentNewWorkorder dnw = new DocumentNewWorkorder(workteam, order)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    dnw.ShowDialog();
                    InitializeGrid();
                }
            }
        }

        private void DocumentNewWorkorder(object sender, RoutedEventArgs e)
        {
            DocumentNewWorkorder dnw = new DocumentNewWorkorder(workteam)
            {
                Owner = Window.GetWindow(this)
            };
            dnw.ShowDialog();

            InitializeGrid();
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
                            Owner = Window.GetWindow(this)
                        };
                    }
                    else if (datePicked.DayOfWeek == DayOfWeek.Saturday)
                    {
                        ao = new AddOffday(workteam, datePicked, datePicked.AddDays(1), OffdayReason.Weekend)
                        {
                            Owner = Window.GetWindow(this)
                        };
                    }
                    else if (datePicked.DayOfWeek == DayOfWeek.Sunday)
                    {
                        ao = new AddOffday(workteam, datePicked.AddDays(-1), datePicked, OffdayReason.Weekend)
                        {
                            Owner = Window.GetWindow(this)
                        };
                    }
                    else
                    {
                        ao = new AddOffday(workteam, datePicked, datePicked, OffdayReason.Helligdag)
                        {
                            Owner = Window.GetWindow(this)
                        };
                    }

                    ao.ShowDialog();
                    InitializeGrid();
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
                            InitializeGrid();
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
                    if (Window.GetWindow(this) is Dashboard dashboard)
                    {
                        dashboard.UpdateWorkteams();
                        dashboard.UpdateContainer();
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void EditWorkteamForeman(object sender, RoutedEventArgs e)
        {
            EditWorkteamForeman ewf = new EditWorkteamForeman(workteam)
            {
                Owner = Window.GetWindow(this)
            };
            ewf.ShowDialog();

            if (Window.GetWindow(this) is Dashboard dashboard)
            {
                dashboard.UpdateWorkteams();
            }
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
                    InitializeGrid();
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
                    InitializeGrid();
                }
            }
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
                XFont workteamFont = new XFont("Verdana", 18, XFontStyle.Bold);

                gfx.DrawString(
                    $"{workteam.Foreman}s hold",
                    workteamFont,
                    XBrushes.Black,
                    new XRect(margin, margin, workspace.X, workspace.Y),
                    XStringFormats.TopCenter);

                double y = margin + 30;

                double rowHeight = 20;

                // Start drawing of the orders
                List<Order> orders = controller.GetAllOrdersFromWorkteam(workteam);
                orders = orders.FindAll(o => o.StartDate != null);
                orders = orders.FindAll(o => o.StartDate >= DateTime.Today || workteam.IsAWorkday(o, DateTime.Today));
                orders.Insert(0, null);
                orders.Insert(0, null);
                for (int i = 0; i < orders.Count; i++)
                {
                    double x = margin;

                    for (int j = 2; j < GridTemplate.ColumnDefinitions.Count - 1; j++)
                    {
                        double width = GridTemplate.ColumnDefinitions[j].Width.Value * sizeMultiplier;
                        XPen pen = new XPen(XColors.LightGray, 1);
                        XRect rect = new XRect(x, y, width, rowHeight);

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
                                    rect = new XRect(x, y, width * days, rowHeight);
                                }
                                else if (weeks * 7 - j < 7)
                                {
                                    rect = new XRect(x, y, width * (weeks * 7 - j), rowHeight);
                                }
                                else
                                {
                                    rect = new XRect(x, y, width * 7, rowHeight);
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
                            XRect rect = new XRect(x, y, width, rowHeight);
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

                    y += rowHeight;
                }

                gfx.DrawString(
                    $"Genererede d. {DateTime.Now.ToString()}",
                    defaultFont,
                    XBrushes.Black,
                    new XRect(workspaceOffset.X, workspaceOffset.Y, workspace.X, workspace.Y),
                    XStringFormats.BottomLeft);

                try
                {
                    document.Save(filePath);
                    Process.Start(filePath);
                }
                catch (IOException)
                {
                    MessageBox.Show("Filen du prøvede og overskrive er åben et andet sted.");
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            OnScrollCollection(sender as ScrollViewer);
        }

        private void OnScrollEvent(ScrollViewer scrollViewer)
        {
            if (scrollViewer != ScrollView)
            {
                if (ScrollView.HorizontalOffset != scrollViewer.HorizontalOffset)
                {
                    ScrollView.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
                }
            }
        }
    }
}
