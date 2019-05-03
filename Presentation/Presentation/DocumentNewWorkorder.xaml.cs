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
    /// Interaction logic for DocumentNewWorkorder.xaml
    /// </summary>
    public partial class DocumentNewWorkorder : Window
    {
        private Controller controller = Controller.Instance;
        private Workteam workteam;
        public Order Order;

        public DocumentNewWorkorder(Workteam workteam)
        {
            InitializeComponent();

            this.workteam = workteam;
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            int? orderNumber = null;
            if (!orderNumberInput.Text.Equals(string.Empty))
            {
                if (int.TryParse(orderNumberInput.Text, out int resultOrderNumber))
                {
                    orderNumber = resultOrderNumber;
                }
                else
                {
                    MessageBox.Show("Order Nummer skal være hele tal");
                    return;
                }
            }

            int? area = null;
            if (!areaInput.Text.Equals(string.Empty))
            {
                if (int.TryParse(areaInput.Text, out int resultArea))
                {
                    area = resultArea;
                }
                else
                {
                    MessageBox.Show("m2 skal være hele tal");
                    return;
                }
            }

            int? amount = null;
            if (!amountInput.Text.Equals(string.Empty))
            {
                if (int.TryParse(amountInput.Text, out int resultAmount))
                {
                    amount = resultAmount;
                }
                else
                {
                    MessageBox.Show("Tons (Mængde) skal være hele tal");
                    return;
                }
            }

            /*Customer customer;
            if (customerInput.Text != string.Empty && !controller.CustomerExistsByName(customerInput.Text))
            {
                controller.CreateCustomer(customerInput.Text);
            }
            customer = controller.GetCustomerByName(customerInput.Text);

            AsphaltCompany asphaltWork;
            if (AsphaltWorkInput.Text != string.Empty && !controller.AsphaltWorkExistsByName(AsphaltWorkInput.Text))
            {
                controller.CreateAsphaltWork(AsphaltWorkInput.Text);
            }
            asphaltWork = controller.GetAsphaltWorkByName(AsphaltWorkInput.Text);

            Machine machine;
            if (machineInput.Text != string.Empty && !controller.MachineExistsByName(machineInput.Text))
            {
                controller.CreateMachine(machineInput.Text);
            }
            machine = controller.GetMachineByName(machineInput.Text);*/

            controller.CreateOrder(workteam, orderNumber, addressInput.Text, remarkInput.Text, area, amount, prescriptionInput.Text, deadlineInput.SelectedDate);

            Close();
        }
    }
}
