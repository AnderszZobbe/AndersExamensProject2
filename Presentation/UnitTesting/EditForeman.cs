using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Application_layer;
using Domain.Exceptions;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class EditForeman
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void EditNameOfForemanOnAWorkteam()
        {
            string oldName = "EditForman1";
            string newName = "EditForman2";

            Workteam workteam = controller.CreateWorkteam(oldName);
            controller.UpdateWorkteamForeman(workteam, newName);

            Assert.AreEqual(newName, workteam.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman3");
            controller.UpdateWorkteamForeman(workteam, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman4");
            controller.CreateWorkteam("EditForman5");

            controller.UpdateWorkteamForeman(workteam, "EditForman5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman6");
            controller.UpdateWorkteamForeman(workteam, "");
        }

        [TestMethod]
        public void CangeToSameName()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman7");
            controller.UpdateWorkteamForeman(workteam, "EditForman7");
        }
    }
}
