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
    /// Interaction logic for DocumentNewWorkorder.xaml
    /// </summary>
    public partial class DocumentNewWorkorder : Window
    {
        private readonly Controller controller = Controller.Instance;
        private readonly Workteam workteam;
        private readonly Order previousOrder;
        public Order Order;

        public DocumentNewWorkorder(Workteam workteam, Order order = null)
        {
            InitializeComponent();

            this.workteam = workteam;

            previousOrder = order;

            if (previousOrder != null)
            {
                orderNumberInput.Text = previousOrder.OrderNumber.ToString() ?? "";
                customerInput.Text = previousOrder.Customer;
                machineInput.Text = previousOrder.Machine;
                addressInput.Text = previousOrder.Address;
                remarkInput.Text = previousOrder.Remark;
                areaInput.Text = previousOrder.Area.ToString() ?? "";
                amountInput.Text = previousOrder.Amount.ToString() ?? "";
                prescriptionInput.Text = previousOrder.Prescription;
                AsphaltWorkInput.Text = previousOrder.AsphaltWork;
                deadlineInput.SelectedDate = previousOrder.Deadline;

                foreach (Assignment assignment in controller.GetAllAssignmentsFromOrder(order))
                {
                    AddWorkform(assignment.Workform, assignment.Duration);
                }

                DeleteOrderButton.Visibility = Visibility.Visible;
                DeleteOrderButton.DataContext = previousOrder;
            }
        }

        private int? ParseToIntOrNull(string s)
        {
            int? nullNumber = null;
            if (!s.Equals(string.Empty))
            {
                nullNumber = int.Parse(s);
            }
            return nullNumber;
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            int? orderNumber;
            try
            {
                orderNumber = ParseToIntOrNull(orderNumberInput.Text);
            }
            catch
            {
                MessageBox.Show("Order Nummer skal være hele tal");
                return;
            }

            int? area;
            try
            {
                area = ParseToIntOrNull(areaInput.Text);
            }
            catch
            {
                MessageBox.Show("m2 skal være hele tal");
                return;
            }

            int? amount;
            try
            {
                amount = ParseToIntOrNull(amountInput.Text);
            }
            catch
            {
                MessageBox.Show("Tons (Mængde) skal være hele tal");
                return;
            }

            foreach (Grid assignment in AssignmentsStackPanel.Children)
            {
                try
                {
                    if(ParseToIntOrNull(((TextBox)assignment.Children[5]).Text) == null)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    MessageBox.Show("Én af arbejdsformerne har ikke tal som antal dage");
                    return;
                }
            }

            Order holdOrder;
            if (previousOrder == null)
            {
                holdOrder = controller.CreateOrder(workteam, orderNumber, addressInput.Text, remarkInput.Text, area, amount, prescriptionInput.Text, deadlineInput.SelectedDate, null, customerInput.Text, machineInput.Text, AsphaltWorkInput.Text);
            }
            else
            {
                controller.UpdateOrder(previousOrder, orderNumber, addressInput.Text, remarkInput.Text, area, amount, prescriptionInput.Text, deadlineInput.SelectedDate, previousOrder.StartDate, customerInput.Text, machineInput.Text, AsphaltWorkInput.Text);
                holdOrder = previousOrder;
                controller.DeleteAllAssignmentsFromOrder(holdOrder);
            }

            foreach (Grid assignment in AssignmentsStackPanel.Children)
            {
                int duration = ParseToIntOrNull(((TextBox)assignment.Children[5]).Text).Value - 1;
                Workform workform = (Workform)((ComboBox)assignment.Children[4]).SelectedItem;

                controller.CreateAssignment(holdOrder, workform, duration);
            }

            Close();
        }

        private void RemoveAssignment(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Grid assignmentGrid)
                {
                    AssignmentsStackPanel.Children.Remove(assignmentGrid);
                }
            }
        }

        private void MoveAssignmentUp(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Grid assignmentGrid)
                {
                    int currentWorkformIndex = AssignmentsStackPanel.Children.IndexOf(assignmentGrid);
                    if (currentWorkformIndex == 0)
                    {
                        return;
                    }
                    AssignmentsStackPanel.Children.Remove(assignmentGrid);
                    AssignmentsStackPanel.Children.Insert(currentWorkformIndex - 1, assignmentGrid);
                }
            }
        }

        private void MoveAssignmentDown(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Grid assignmentGrid)
                {
                    int currentWorkformIndex = AssignmentsStackPanel.Children.IndexOf(assignmentGrid);
                    if (currentWorkformIndex == AssignmentsStackPanel.Children.Count - 1)
                    {
                        return;
                    }
                    AssignmentsStackPanel.Children.Remove(assignmentGrid);
                    AssignmentsStackPanel.Children.Insert(currentWorkformIndex + 1, assignmentGrid);
                }
            }
        }

        private void AddWorkform(object sender, RoutedEventArgs e)
        {
            AddWorkform(Workform.Dag, 0);
        }

        private void AddWorkform(Workform workform, int duration)
        {
            Grid grid = new Grid();
            AssignmentsStackPanel.Children.Add(grid);
            for (int i = 0; i < 2; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });

            Button btnUp = new Button
            {
                Content = "Flyt op",
                DataContext = grid
            };
            btnUp.Click += MoveAssignmentUp;
            grid.Children.Add(btnUp);

            Button btnDown = new Button
            {
                Content = "Flyt ned",
                DataContext = grid
            };
            btnDown.Click += MoveAssignmentDown;
            grid.Children.Add(btnDown);
            Grid.SetRow(btnDown, 1);

            Label workformLabel = new Label
            {
                Padding = new Thickness(0),
                Content = "Arbejdsform:"
            };
            grid.Children.Add(workformLabel);
            Grid.SetColumn(workformLabel, 1);

            Label durationLabel = new Label
            {
                Padding = new Thickness(0),
                Content = "Dage:"
            };
            grid.Children.Add(durationLabel);
            Grid.SetColumn(durationLabel, 2);



            ComboBox workformComboBox = new ComboBox
            {
            };
            workformComboBox.ItemsSource = Enum.GetValues(typeof(Workform));
            workformComboBox.SelectedItem = workform;
            grid.Children.Add(workformComboBox);
            Grid.SetColumn(workformComboBox, 1);
            Grid.SetRow(workformComboBox, 1);

            TextBox durationBox = new TextBox
            {
                Padding = new Thickness(0),
                Text = (duration + 1).ToString(),
            };
            grid.Children.Add(durationBox);
            Grid.SetColumn(durationBox, 2);
            Grid.SetRow(durationBox, 1);


            Button btnDel = new Button
            {
                Content = "Fjern",
                DataContext = grid,
                VerticalAlignment = VerticalAlignment.Center
            };
            btnDel.Click += RemoveAssignment;
            grid.Children.Add(btnDel);
            Grid.SetColumn(btnDel, 3);
            Grid.SetRowSpan(btnDel, 2);
        }

        private void DeleteOrderConfirm(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is Order order)
                {
                    MessageBoxResult result = MessageBox.Show("Vil du slette denne arbejdsorder?", "Slet arbejdsorder", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            controller.DeleteOrder(workteam, order);
                            Close();
                            break;
                    }
                }
            }
        }
    }
}
