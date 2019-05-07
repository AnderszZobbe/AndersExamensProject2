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
            string foreman = "Adam";
            
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateAndGetOrder(workteam,1234,"","",1234,123,"",DateTime.Today);

            Assert.AreEqual(true, controller.DeleteOrder(workteam, order));
            Assert.AreEqual(false, controller.GetAllOrdersByWorkteam(workteam).Exists(o => o == order));
        }
        [TestMethod]
        public void TestDeletionOfNullObject()
        {
            string foreman = "BOB";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            Order order = new Order();
            Assert.AreEqual(false, controller.DeleteOrder(workteam, order));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteOrder(null, null));
        }
    }
    }
