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
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("OrderExists");
        }

        [TestMethod]
        public void OrderDoesExist()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(true, controller.OrderExists(order));
        }

        [TestMethod]
        public void OrderDoesntExists()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.DeleteOrder(workteam, order);
            Assert.AreEqual(false, controller.OrderExists(order));
        }
    }
}
