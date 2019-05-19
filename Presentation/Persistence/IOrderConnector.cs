using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IOrderConnector
    {
        KeyValuePair<Order, int> CreateOrder(int workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);

        Dictionary<Order, int> GetAllOrdersByWorkteam(int workteam);

        void SwapOrderPriorities(int firstOrder, int secondOrder);
        void UpdateOrderOrderNumber(int order, int? orderNumber);
        void UpdateOrderAddress(int order, string address);
        void UpdateOrderRemark(int order, string remark);
        void UpdateOrderArea(int order, int? area);
        void UpdateOrderAmount(int order, int? amount);
        void UpdateOrderPrescription(int order, string prescription);
        void UpdateOrderDeadline(int order, DateTime? deadline);
        void UpdateOrderStartDate(int order, DateTime? startDate);
        void UpdateOrderCustomer(int order, string customer);
        void UpdateOrderMachine(int order, string machine);
        void UpdateOrderAsphaltWork(int order, string asphaltWork);

        void DeleteOrder(int order);
    }
}
