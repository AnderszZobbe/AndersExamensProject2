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
    public class DeleteAllAssignmentsFromOrder
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void TestSuccesfulDeletion()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAllAssignmentsFromOrder1");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 2);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 2);

            Assert.AreEqual(true, controller.DeleteAllAssignmentsFromOrder(order));
            Assert.AreEqual(0, controller.GetAllAssignmentsFromOrder(order).Count);

        }

        [TestMethod]
        public void TestDeletionWithNoAssignments()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAllAssignmentsFromOrder3");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);

            Assert.AreEqual(true, controller.DeleteAllAssignmentsFromOrder(order));
            Assert.AreEqual(0, controller.GetAllAssignmentsFromOrder(order).Count);

        }

        [TestMethod]
        public void ReturnTrueWithNoneInIt()
        {
            Workteam workteam = controller.CreateWorkteam("TestDeletionOfNullObject");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(true, controller.DeleteAllAssignmentsFromOrder(order));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeletionOfNullObject()
        {
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            controller.DeleteAllAssignmentsFromOrder(order);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            controller.DeleteAllAssignmentsFromOrder(null);
        }
    }
    }
