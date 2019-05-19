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
    public class DeleteWorkteam
    {
        Controller controller;
        Workteam workteam1, workteam2, workteam3;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam1 = controller.CreateWorkteam("DeleteWorkteam1");
            workteam2 = controller.CreateWorkteam("DeleteWorkteam2");
            workteam3 = controller.CreateWorkteam("DeleteWorkteam3");
            controller.CreateOrder(workteam1, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam1, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam1, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam3, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam3, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam3, null, null, null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void CanDelete()
        {
            controller.DeleteWorkteam(workteam1);
            controller.DeleteWorkteam(workteam2);
            controller.DeleteWorkteam(workteam3);
        }

        [TestMethod]
        public void ReturnTrue()
        {
            Assert.IsTrue(controller.DeleteWorkteam(workteam1));
        }

        [TestMethod]
        public void ReturnFalse()
        {
            Assert.IsFalse(controller.DeleteWorkteam(null));
        }

        [TestMethod]
        public void SuccesfulDeletionCount()
        {
            controller.DeleteWorkteam(workteam1);
            Assert.AreEqual(2, controller.GetAllWorkteams().Count);
        }

        [TestMethod]
        public void SuccesfulDeletion()
        {
            controller.DeleteWorkteam(workteam1);
            Assert.AreEqual(workteam2, controller.GetAllWorkteams()[0]);
        }
    }
}
