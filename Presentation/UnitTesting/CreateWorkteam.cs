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
        string foreman;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
            foreman = "CreateWorkteam";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanCreate()
        {
            workteam = controller.CreateWorkteam(foreman);
        }

        [TestMethod]
        public void ReturnWorkteam()
        {
            workteam = controller.CreateWorkteam(foreman);
            Assert.IsNotNull(workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DuplicateWorkteamForeman()
        {
            controller.CreateWorkteam(foreman);
            controller.CreateWorkteam(foreman);
        }

        [TestMethod]
        public void WorkteamCount()
        {
            controller.CreateWorkteam(foreman);
            Assert.AreEqual(1, controller.GetAllWorkteams().Count);
        }

        [TestMethod]
        public void CanFindWorkteam()
        {
            Workteam workteam = controller.CreateWorkteam(foreman);
            Assert.IsTrue(controller.GetAllWorkteams().Contains(workteam));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionEmptyName()
        {
            controller.CreateWorkteam(string.Empty);
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
