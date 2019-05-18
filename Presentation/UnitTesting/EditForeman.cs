using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Application_layer;
using Application_layer.Exceptions;
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
            Controller.Connector = new TestManagerAndProvider();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void EditNameOfForemanOnAWorkteam()
        {
            string oldName = "EditForman1";
            string newName = "EditForman2";

            Workteam workteam = controller.CreateWorkteam(oldName);
            controller.EditForeman(workteam, newName);

            Assert.AreEqual(newName, workteam.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman3");
            controller.EditForeman(workteam, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman4");
            controller.CreateWorkteam("EditForman5");

            controller.EditForeman(workteam, "EditForman5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman6");
            controller.EditForeman(workteam, "");
        }

        [TestMethod]
        public void CangeToSameName()
        {
            Workteam workteam = controller.CreateWorkteam("EditForman7");
            controller.EditForeman(workteam, "EditForman7");
        }


    }
}
