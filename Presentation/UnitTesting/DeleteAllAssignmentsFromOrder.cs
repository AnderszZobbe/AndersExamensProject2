using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Application_layer.Exceptions;
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
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void TestSuccesfulDeletion()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAllAssignmentsFromOrder1");
            Order order = controller.CreateOrder(workteam,1234,"","",1234,123,"",DateTime.Today, null, null, null, null);
            controller.CreateAssignment(order, 2, Workform.Dag);
            controller.CreateAssignment(order, 2, Workform.Dag);

            Assert.AreEqual(true, controller.DeleteAllAssignmentsFromOrder(order));
            Assert.AreEqual(0, order.assignments.Count);

        }

        [TestMethod]
        public void TestDeletionWithNoAssignments()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAllAssignmentsFromOrder3");
            Order order = controller.CreateOrder(workteam, 1234, "", "", 1234, 123, "", DateTime.Today, null, null, null, null);

            Assert.AreEqual(true, controller.DeleteAllAssignmentsFromOrder(order));
            Assert.AreEqual(0, order.assignments.Count);

        }

        [TestMethod]
        public void TestDeletionOfNullObject()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAllAssignmentsFromOrder2");
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            Assert.AreEqual(false, controller.DeleteAllAssignmentsFromOrder(order));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteAllAssignmentsFromOrder(null));
        }
    }
    }
