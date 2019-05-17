using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public interface IConnector
    {
        // Create

        /// <summary>
        /// Constructs a workteam
        /// </summary>
        /// <param name="foreman"></param>
        /// <returns></returns>
        Workteam CreateWorkteam(string foreman);

        /// <summary>
        /// Constructs an offday to a workteam
        /// </summary>
        /// <param name="workteam"></param>
        /// <param name="reason"></param>
        /// <param name="startDate"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration);

        /// <summary>
        /// Constructs an order to a workteam
        /// </summary>
        /// <param name="workteam"></param>
        /// <param name="orderNumber"></param>
        /// <param name="address"></param>
        /// <param name="remark"></param>
        /// <param name="area"></param>
        /// <param name="amount"></param>
        /// <param name="prescription"></param>
        /// <param name="deadline"></param>
        /// <param name="startDate"></param>
        /// <param name="customer"></param>
        /// <param name="machine"></param>
        /// <param name="asphaltWork"></param>
        /// <returns></returns>
        Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);

        /// <summary>
        /// Constructs an assignment to an order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="workform"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        Assignment CreateAssignment(Order order, Workform workform, int duration);

        // Read
        // > Get All

        List<Workteam> GetAllWorkteams();
        List<Offday> GetAllOffdays();
        List<Order> GetAllOrders();
        List<Assignment> GetAllAssignments();

        // > Exists

        bool WorkteamExists(Workteam workteam);
        bool OffdayExists(Offday offday);
        bool OrderExists(Order order);
        bool AssignmentExists(Assignment assignment);

        // > Fill

        void FillWorkteamWithOrders(Workteam workteam);
        void FillWorkteamWithOffdays(Workteam workteam);
        void FillOrderWithAssignments(Order order);

        // Update

        void UpdateWorkteam(Workteam workteam, string foreman);
        void UpdateOrderStartDate(Order order, DateTime? startDate);
        void SwitchOrdersPriority(Order firstOrder, Order secondOrder);
        void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork);

        // Delete

        /// <summary>
        /// Deletes the workteam along with all things associated with it
        /// </summary>
        /// <param name="workteam"></param>
        /// <returns></returns>
        bool DeleteWorkteam(Workteam workteam);

        /// <summary>
        /// Deletes the Offday
        /// </summary>
        /// <param name="workteam"></param>
        /// <param name="offday"></param>
        /// <returns></returns>
        bool DeleteOffday(Workteam workteam, Offday offday);

        /// <summary>
        /// Deletes the order along with all assignments associated with it
        /// </summary>
        /// <param name="workteam"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        bool DeleteOrder(Workteam workteam, Order order);

        /// <summary>
        /// Deletes the Assignment
        /// </summary>
        /// <param name="order"></param>
        /// <param name="assignment"></param>
        /// <returns></returns>
        bool DeleteAssignment(Order order, Assignment assignment);
    }
}
