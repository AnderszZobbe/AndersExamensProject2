using System;
using Application_layer;
using Domain;
using Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class OrderExists
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void OrderDoesExist()
        {
            Workteam workteam = controller.CreateWorkteam("OrderDoesExist");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(true, controller.OrderExists(order));
        }

        [TestMethod]
        public void OrderDoesntExists()
        {
            Workteam workteam = controller.CreateWorkteam("OrderDoesntExists");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.DeleteOrder(workteam, order);
            Assert.AreEqual(false, controller.OrderExists(order));
        }
    }
}
