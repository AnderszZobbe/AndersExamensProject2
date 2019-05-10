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
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void ReturnWorkteam()
        {
            string foreman = "Sven";

            Workteam workteam = controller.CreateWorkteam(foreman);

            Assert.IsNotNull(workteam);
        }

        [TestMethod]
        public void EstablishMoreWorkteam()
        {
            string foreman = "Knud";

            Workteam workteam = controller.CreateWorkteam(foreman);

            Workteam workteamFound = controller.GetAllWorkteams().Find(o => o.Foreman == workteam.Foreman);

            Assert.AreEqual(workteam, workteamFound);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateName()
        {
            controller.CreateWorkteam("Knud");
            controller.CreateWorkteam("Knud");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionEmptyName()
        {
            string foreman = "";
            controller.CreateWorkteam(foreman);
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
