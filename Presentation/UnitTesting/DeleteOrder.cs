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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSuccesfulDeletion()
        {
            string foreman = "Adam";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOrder(workteam,1234,"","",1234,123,"",DateTime.Today);

            Order orders = controller.GetAllOrdersByWorkteam(workteam)[0];
            Assert.AreEqual(true, controller.DeleteOrder(orders));
            orders = controller.GetAllOrdersByWorkteam(workteam)[0];
        }
        [TestMethod]
        public void TestDeletionOfNullObject()
        {
            string foreman = "BOB";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            Order order = new Order(workteam);
            Assert.AreEqual(false, controller.DeleteOrder(order));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteOrder(null));
        }
    }
    }
