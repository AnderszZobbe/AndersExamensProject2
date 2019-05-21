using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class EditOrder
    {
        Controller controller;
        Workteam workteam;
        Order order1, order2, order3;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("EditOrder");
            order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            order2 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            order3 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanEdit()
        {
            controller.UpdateOrder(order1, 1, "address", "remark", 10, 10, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
        }

        [TestMethod]
        public void SavesChanges()
        {
            controller.UpdateOrder(order1, 1, "address", "remark", 10, 100, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
            Assert.AreEqual(1, order1.OrderNumber);
            Assert.AreEqual("address", order1.Address);
            Assert.AreEqual("remark", order1.Remark);
            Assert.AreEqual(10, order1.Area);
            Assert.AreEqual(100, order1.Amount);
            Assert.AreEqual("prescription", order1.Prescription);
            Assert.AreEqual(DateTime.Now.Date, order1.Deadline);
            Assert.AreEqual(DateTime.Now.Date, order1.StartDate);
            Assert.AreEqual("customer", order1.Customer);
            Assert.AreEqual("machine", order1.Machine);
            Assert.AreEqual("asphaltWork", order1.AsphaltWork);
        }

        [TestMethod]
        public void SavesChangesToNull()
        {
            controller.UpdateOrder(order1, 1, "address", "remark", 10, 100, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
            controller.UpdateOrder(order1, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(null, order1.OrderNumber);
            Assert.AreEqual(null, order1.Address);
            Assert.AreEqual(null, order1.Remark);
            Assert.AreEqual(null, order1.Area);
            Assert.AreEqual(null, order1.Amount);
            Assert.AreEqual(null, order1.Prescription);
            Assert.AreEqual(null, order1.Deadline);
            Assert.AreEqual(null, order1.StartDate);
            Assert.AreEqual(null, order1.Customer);
            Assert.AreEqual(null, order1.Machine);
            Assert.AreEqual(null, order1.AsphaltWork);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionDuplicateOrder()
        {
            controller.CreateOrder(workteam, 1, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrder(order1, 1, null, null, null, null, null, null, null, null, null, null);
        }
    }
}
