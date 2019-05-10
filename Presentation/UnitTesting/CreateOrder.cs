using System;
using Application_layer;
using Application_layer.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateOrder
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
            string foreman = "Alpha";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOrder(workteam,8,"","",1,1,"",DateTime.Today);
            controller.FillWorkteamWithOrders(workteam);
            Order order = workteam.orders[0];

            Assert.IsNotNull(order);
        }

        

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOrder()
        {
            string foreman = "Bravo";
            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
            controller.CreateOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyNoName()
        {
            controller.CreateOrder(null, 2, "", "", 1, 1, "", DateTime.Today);
        }

        [TestMethod]
        public void CanAddTwoOrdersWithNull()
        {
            Workteam workteam = controller.CreateWorkteam("Frank");
            controller.CreateOrder(workteam, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam, null, null, null, null, null, null, null);
        }
    }
}
