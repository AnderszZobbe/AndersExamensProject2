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
    public class DeleteWorkteam
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void TestSuccesfulDeletion()
        {
            string foreman = "Derp";
            controller.CreateWorkteam(foreman);

            Workteam workteam = controller.GetWorkteamByName(foreman);
            Assert.AreEqual(true, controller.DeleteWorkteam(workteam));
            workteam = controller.GetWorkteamByName(foreman);
        }

        [TestMethod]
        public void DeleteNonExistentWorkteam()
        {
            string foreman = "Gert";

            Workteam workteam = new Workteam(foreman);

            Assert.AreEqual(false, controller.DeleteWorkteam(workteam));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            controller.DeleteWorkteam(null);
        }
    }
}
