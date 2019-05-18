using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Application_layer.Exceptions;
using Persistence;
using Domain.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class DeleteWorkteam
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new TestManagerAndProvider();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void TestSuccesfulDeletion()
        {
            Workteam workteam = controller.CreateWorkteam("Derp");
            Assert.AreEqual(true, controller.DeleteWorkteam(workteam));
            Assert.AreEqual(false, controller.DeleteWorkteam(workteam));
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
