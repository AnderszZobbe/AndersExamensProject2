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
        public Order Order;

        public DocumentNewWorkorder(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;
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
                area = ParseToIntOrNull(orderNumberInput.Text);
            }
            catch
            {
                MessageBox.Show("m2 skal være hele tal");
                return;
            }

            int? amount;
            try
            {
                amount = ParseToIntOrNull(orderNumberInput.Text);
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

            Order orderCreated = controller.CreateOrder(workteam, orderNumber, addressInput.Text, remarkInput.Text, area, amount, prescriptionInput.Text, deadlineInput.SelectedDate, null, null, null, null);


            foreach (Grid assignment in AssignmentsStackPanel.Children)
            {
                int duration = ParseToIntOrNull(((TextBox)assignment.Children[5]).Text).Value - 1;
                Workform workform = (Workform)((ComboBox)assignment.Children[4]).SelectedItem;

                controller.CreateAssignment(orderCreated, duration, workform);
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
            AddWorkform();
        }

        private void AddWorkform()
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

            Label workform = new Label
            {
                Padding = new Thickness(0),
                Content = "Arbejdsform:"
            };
            grid.Children.Add(workform);
            Grid.SetColumn(workform, 1);

            Label duration = new Label
            {
                Padding = new Thickness(0),
                Content = "Dage:"
            };
            grid.Children.Add(duration);
            Grid.SetColumn(duration, 2);



            ComboBox workformComboBox = new ComboBox
            {
            };
            workformComboBox.ItemsSource = Enum.GetValues(typeof(Workform));
            workformComboBox.SelectedItem = Workform.Dag;
            grid.Children.Add(workformComboBox);
            Grid.SetColumn(workformComboBox, 1);
            Grid.SetRow(workformComboBox, 1);

            TextBox durationBox = new TextBox
            {
                Padding = new Thickness(0),
                Text = "1",
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
    }
}
