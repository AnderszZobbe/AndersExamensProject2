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
    public class DeleteAssignment
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
            Workteam workteam = controller.CreateWorkteam("DeleteAssignment1");
            Order order = controller.CreateOrder(workteam,1234,"","",1234,123,"",DateTime.Today, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 1, Workform.Dag);
            Assert.IsTrue(controller.DeleteAssignment(order, assignment));
            Assert.AreEqual(0, order.assignments.Count);
          
        }

        [TestMethod]
        public void TestDeletionOfAssignementOnWrongOrder()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteAssignment2");
            Workteam workteamtwo = controller.CreateWorkteam("DeleteAssignment3");
            Order order = controller.CreateOrder(workteam, 1234, "", "", 1234, 123, "", DateTime.Today, null, null, null, null);
            Order ordertwo = controller.CreateOrder(workteamtwo, 1234, "", "", 1234, 123, "", DateTime.Today, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 1, Workform.Dag);
            Assert.IsFalse(controller.DeleteAssignment(ordertwo, assignment));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteAssignment(null, null));
        }
    }
    }
