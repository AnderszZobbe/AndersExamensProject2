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
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void EditNameOfForemanOnAWorkteam()
        {
            string oldName = "Bans";
            string newName = "Fans";

            Workteam workteam = controller.CreateWorkteam(oldName);
            controller.EditForeman(workteam, newName);

            Assert.AreEqual(newName, workteam.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            Workteam workteam = controller.CreateWorkteam("Hans");
            controller.EditForeman(workteam, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            Workteam workteam = controller.CreateWorkteam("Tom");
            Workteam workteam2 = controller.CreateWorkteam("Jan");

            controller.EditForeman(workteam, "Jan");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            Workteam workteam = controller.CreateWorkteam("Jim");
            controller.EditForeman(workteam, "");
        }
    }
}
