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
    public class DeleteAssignment
    {
        Controller controller;
        Workteam workteam;
        Order order;
        Assignment assignment1, assignment2, assignment3;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("DeleteAssignment");
            order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            assignment1 = controller.CreateAssignment(order, Workform.Dagsarbejde, 0);
            assignment2 = controller.CreateAssignment(order, Workform.Dagsarbejde, 0);
            assignment3 = controller.CreateAssignment(order, Workform.Dagsarbejde, 0);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanDelete()
        {
            controller.DeleteAssignment(order, assignment1);
            controller.DeleteAssignment(order, assignment2);
            controller.DeleteAssignment(order, assignment3);
        }

        [TestMethod]
        public void ReturnTrue()
        {
            Assert.IsTrue(controller.DeleteAssignment(order, assignment1));
        }

        [TestMethod]
        public void ReturnFalse()
        {
            Assert.IsFalse(controller.DeleteAssignment(order, null));
        }

        [TestMethod]
        public void SuccesfulDeletionCount()
        {
            controller.DeleteAssignment(order, assignment2);
            Assert.AreEqual(2, controller.GetAllAssignmentsFromOrder(order).Count);
        }

        [TestMethod]
        public void SuccesfulDeletion()
        {
            controller.DeleteAssignment(order, assignment1);
            Assert.AreEqual(assignment2, controller.GetAllAssignmentsFromOrder(order)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            controller.DeleteAssignment(null, assignment1);
        }
    }
}
