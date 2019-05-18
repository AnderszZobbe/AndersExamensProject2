using System;
using Application_layer;
using Application_layer.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateWorkteam
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new TestManagerAndProvider();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void ReturnWorkteam()
        {
            Assert.IsNotNull(controller.CreateWorkteam("Test"));
        }

        [TestMethod]
        public void EstablishMoreWorkteam()
        {
            Workteam workteam = controller.CreateWorkteam("Test");

            Workteam workteamFound = controller.GetAllWorkteams().Find(o => o.Foreman == workteam.Foreman);

            Assert.AreEqual(workteam, workteamFound);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateName()
        {
            controller.CreateWorkteam("Test");
            controller.CreateWorkteam("Test");
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
