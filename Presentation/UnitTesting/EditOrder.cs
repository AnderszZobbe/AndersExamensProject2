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
        Order order;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("EditOrder");
            order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanEdit()
        {
            controller.UpdateOrder(order, 1, "address", "remark", 10, 10, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
        }

        [TestMethod]
        public void SavesChanges()
        {
            controller.UpdateOrder(order, 1, "address", "remark", 10, 100, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
            /*order.OrderNumber = 1;
            order.Address = "address";
            order.Remark = "remark";
            order.Area = 10;
            order.Amount = 100;
            order.Prescription = "prescription";
            order.Deadline = DateTime.Now;
            order.StartDate = DateTime.Now;
            order.Customer = "customer";
            order.Machine = "machine";
            order.AsphaltWork = "asphaltWork";*/
            Assert.AreEqual(1, order.OrderNumber);
            Assert.AreEqual("address", order.Address);
            Assert.AreEqual("remark", order.Remark);
            Assert.AreEqual(10, order.Area);
            Assert.AreEqual(100, order.Amount);
            Assert.AreEqual("prescription", order.Prescription);
            Assert.AreEqual(DateTime.Today, order.Deadline);
            Assert.AreEqual(DateTime.Today, order.StartDate);
            Assert.AreEqual("customer", order.Customer);
            Assert.AreEqual("machine", order.Machine);
            Assert.AreEqual("asphaltWork", order.AsphaltWork);
        }

        [TestMethod]
        public void SavesChangesToNull()
        {
            controller.UpdateOrder(order, 1, "address", "remark", 10, 100, "prescription", DateTime.Now, DateTime.Now, "customer", "machine", "asphaltWork");
            controller.UpdateOrder(order, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(null, order.OrderNumber);
            Assert.AreEqual(null, order.Address);
            Assert.AreEqual(null, order.Remark);
            Assert.AreEqual(null, order.Area);
            Assert.AreEqual(null, order.Amount);
            Assert.AreEqual(null, order.Prescription);
            Assert.AreEqual(null, order.Deadline);
            Assert.AreEqual(null, order.StartDate);
            Assert.AreEqual(null, order.Customer);
            Assert.AreEqual(null, order.Machine);
            Assert.AreEqual(null, order.AsphaltWork);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionDuplicateOrder()
        {
            controller.CreateOrder(workteam, 1, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrder(order, 1, null, null, null, null, null, null, null, null, null, null);
        }
    }
}
