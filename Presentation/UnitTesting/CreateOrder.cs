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

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOrder(workteam,1,"","",1,1,"",DateTime.Today);
            Order order = controller.GetAllOrdersByWorkteam(workteam)[0];

            Assert.IsNotNull(order);
        }

        

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOrder()
        {
            string foreman = "Bravo";
            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
            controller.CreateOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyNoName()
        {
            controller.CreateOrder(null, 1, "", "", 1, 1, "", DateTime.Today);
        }

    }
}
