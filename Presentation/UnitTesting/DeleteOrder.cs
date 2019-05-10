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
    public class DeleteOrder
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
            Workteam workteam = controller.CreateWorkteam("Adam");
            Order order = controller.CreateOrder(workteam,1234,"","",1234,123,"",DateTime.Today);
            
            Assert.AreEqual(true, controller.DeleteOrder(workteam, order));
            controller.FillWorkteamWithOrders(workteam);
            Assert.AreEqual(false, workteam.orders.Exists(o => o == order));
        }

        [TestMethod]
        public void TestDeletionOfNullObject()
        {
            Workteam workteam = controller.CreateWorkteam("TestDeletionOfNullObject");
            Order order = new Order();
            Assert.AreEqual(false, controller.DeleteOrder(workteam, order));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteOrder(null, null));
        }
    }
    }
