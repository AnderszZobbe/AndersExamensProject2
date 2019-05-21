using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Domain.Exceptions;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class DeleteOrder
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
            workteam = controller.CreateWorkteam("DeleteAssignment");
            order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            order2 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            order3 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order1, Workform.Dag, 0);
            controller.CreateAssignment(order1, Workform.Dag, 0);
            controller.CreateAssignment(order1, Workform.Dag, 0);
            controller.CreateAssignment(order2, Workform.Dag, 0);
            controller.CreateAssignment(order2, Workform.Dag, 0);
            controller.CreateAssignment(order2, Workform.Dag, 0);
            controller.CreateAssignment(order3, Workform.Dag, 0);
            controller.CreateAssignment(order3, Workform.Dag, 0);
            controller.CreateAssignment(order3, Workform.Dag, 0);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanDelete()
        {
            controller.DeleteOrder(workteam, order1);
            controller.DeleteOrder(workteam, order2);
            controller.DeleteOrder(workteam, order3);
        }

        [TestMethod]
        public void ReturnTrue()
        {
            Assert.IsTrue(controller.DeleteOrder(workteam, order1));
        }

        [TestMethod]
        public void ReturnFalse()
        {
            Assert.IsFalse(controller.DeleteOrder(workteam, null));
        }

        [TestMethod]
        public void SuccesfulDeletionCount()
        {
            controller.DeleteOrder(workteam, order1);
            Assert.AreEqual(2, controller.GetAllOrdersFromWorkteam(workteam).Count);
        }

        [TestMethod]
        public void SuccesfulDeletion()
        {
            controller.DeleteOrder(workteam, order1);
            Assert.AreEqual(order2, controller.GetAllOrdersFromWorkteam(workteam)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            controller.DeleteOrder(null, order1);
        }
    }
}
