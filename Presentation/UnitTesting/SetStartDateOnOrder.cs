using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class SetStartDateOnOrder
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void SetsCorrectly()
        {
            string foreman = "SetStartDateOnOrder1";

            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 6, "", "", 1, 1, "", null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today);

            Assert.AreEqual(DateTime.Today,order.StartDate);
            controller.UpdateOrderStartDate(order, DateTime.Today.AddDays(1));

            Assert.AreEqual(DateTime.Today.AddDays(1), order.StartDate);
        }

        [TestMethod]
        public void StartDateAfterDeadline()
        {
            string foreman = "SetStartDateOnOrder2";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 6, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today.AddDays(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartDateAtEndOfTime()
        {
            string foreman = "SetStartDateOnOrder3";

            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 6, "", "", 1, 1, "",null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.MaxValue.AddDays(1));
        }
        [TestMethod]
        public void CanNullStartDate()
        {
            string foreman = "SetStartDateOnOrder4";

            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 6, "", "", 1, 1, "", null,DateTime.Today, null, null, null);
            controller.UpdateOrderStartDate(order, null);
            Assert.AreEqual(null, order.StartDate);
        }
    }
}
