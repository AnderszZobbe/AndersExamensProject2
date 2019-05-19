using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateWorkteam
    {
        Controller controller;
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void CanCreate()
        {
            controller.CreateWorkteam("CreateWorkteam");
        }

        [TestMethod]
        public void ReturnWorkteam()
        {
            workteam = controller.CreateWorkteam("CreateWorkteam");
            Assert.IsNotNull(workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DuplicateWorkteamForeman()
        {
            controller.CreateWorkteam("CreateWorkteam");
            controller.CreateWorkteam("CreateWorkteam");
        }

        [TestMethod]
        public void WorkteamCount()
        {
            controller.CreateWorkteam("CreateWorkteam");
            Assert.AreEqual(1, controller.GetAllWorkteams().Count);
        }

        [TestMethod]
        public void CanFindWorkteam()
        {
            Workteam workteam = controller.CreateWorkteam("CreateWorkteam");
            Assert.IsTrue(controller.GetAllWorkteams().Contains(workteam));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionEmptyName()
        {
            controller.CreateWorkteam("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyNoName()
        {
            string foreman = null;
            controller.CreateWorkteam(foreman);
        }
    }
}
