using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Application_layer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class DeleteWorkteam
    {
        [TestMethod]
        public void TestSuccesfulDeletion()
        {
            Controller controller = Controller.Instance;
            string foreman = "Hans";
            controller.CreateWorkteam(foreman);

            Workteam workteam = controller.GetWorkteamByName(foreman);
            Assert.AreEqual(true, controller.DeleteWorkteam(workteam));
            Assert.AreEqual(0, controller.GetAllWorkteams().Count);
        }

        [TestMethod]
        public void DeleteNonExistentWorkteam()
        {
            Controller controller = Controller.Instance;
            string foreman = "Gert";

            Workteam workteam = new Workteam(foreman);

            Assert.AreEqual(false, controller.DeleteWorkteam(workteam));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            Controller controller = Controller.Instance;

            controller.DeleteWorkteam(null);
        }
    }
}
