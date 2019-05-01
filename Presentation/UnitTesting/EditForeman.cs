using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Application_layer;
using Application_layer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class EditForeman
    {
        Controller controller = Controller.Instance;

        [TestMethod]
        public void EditNameOfForemanOnAWorkteam()
        {
            string oldName = "Bans";
            string newName = "Fans";

            controller.CreateWorkteam(oldName);
            Workteam workteam = controller.GetWorkteamByName(oldName);
            controller.EditForeman(newName, workteam);

            Assert.AreEqual(newName, workteam.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            controller.CreateWorkteam("Hans");
            Workteam workteam = controller.GetWorkteamByName("Hans");
            controller.EditForeman(null, workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            controller.CreateWorkteam("Tom");
            controller.CreateWorkteam("Jan");
            Workteam workteam = controller.GetWorkteamByName("Tom");
            Workteam workteam2 = controller.GetWorkteamByName("Jan");

            controller.EditForeman("Jan", workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            controller.CreateWorkteam("Jim");
            Workteam workteam = controller.GetWorkteamByName("Jim");
            controller.EditForeman("", workteam);
        }
    }
}
