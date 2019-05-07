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
            controller.CreateAndGetOrder(workteam,1,"","",1,1,"",DateTime.Today);
            Order order = controller.GetAllOrdersByWorkteam(workteam)[0];

            Assert.IsNotNull(order);
        }

        

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOrder()
        {
            string foreman = "Bravo";
            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateAndGetOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
            controller.CreateAndGetOrder(workteam, 1, "", "", 1, 1, "", DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyNoName()
        {
            controller.CreateAndGetOrder(null, 2, "", "", 1, 1, "", DateTime.Today);
        }

    }
}
