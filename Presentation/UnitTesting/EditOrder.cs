using System;
using Application_layer;
using Application_layer.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class EditOrder
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void ReturnOrder()
        {
            string foreman = "EditOrder1";

            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 6, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
            controller.EditOrder(order,6,"","",1,1,"",DateTime.Today, null, null, null, null);
            Assert.IsNotNull(order);
        }

        

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOrder()
        {
            string foreman = "EditOrder2";
            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOrder(workteam, 7, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
            Order order = controller.CreateOrder(workteam, 0, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
            controller.EditOrder(order, 7, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
        }

        [TestMethod]
        public void ChangesHappen()
        {
            string foreman = "EditOrder3";

            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, 80, "", "", 1, 1, "", DateTime.Today, null, null, null, null);
            controller.EditOrder(order, 9, "1", "1", 2, 2, "1", DateTime.Today.AddDays(1), null, null, null, null);

            Assert.AreEqual(order.OrderNumber, 9);
            Assert.AreEqual(order.Address, "1");
            Assert.AreEqual(order.Remark, "1");
            Assert.AreEqual(order.Area, 2);
            Assert.AreEqual(order.Amount, 2);
            Assert.AreEqual(order.Prescription, "1");
            Assert.AreEqual(order.Deadline, DateTime.Today.AddDays(1));
        }

    }
}
