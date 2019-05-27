using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateOrder
    {
        Controller controller;
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("CreateOrder");
        }

        [TestMethod]
        public void CreateOrderSuccess()
        {
            Assert.IsNotNull(controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null));
        }

        [TestMethod]
        public void CountCorrect()
        {
            controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(1, controller.GetAllOrdersFromWorkteam(workteam).Count);
        }

        [TestMethod]
        public void ReturnOrder()
        {
            controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.IsNotNull(controller.GetAllOrdersFromWorkteam(workteam)[0]);
        }

        [TestMethod]
        public void SameOrder()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(order, controller.GetAllOrdersFromWorkteam(workteam)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOrder()
        {
            controller.CreateOrder(workteam, 1, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam, 1, null, null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyNoName()
        {
            controller.CreateOrder(null, null, null, null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void CanAddTwoOrdersWithNull()
        {
            controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
        }
    }
}
