using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Application_layer;
using Domain.Exceptions;
using Persistence;
using Persistence.DataClasses;

namespace UnitTesting
{
    [TestClass]
    public class EditForeman
    {
        Controller controller;
        Workteam workteam1, workteam2;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam1 = controller.CreateWorkteam("EditForeman1");
            workteam2 = controller.CreateWorkteam("EditForeman2");
        }

        [TestMethod]
        public void EditNameOfForemanOnAWorkteam()
        {
            string newForeman = "newForeman";
            controller.UpdateWorkteam(workteam1, newForeman);

            Assert.AreEqual(newForeman, workteam1.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            controller.UpdateWorkteam(workteam1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            controller.UpdateWorkteam(workteam1, workteam2.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            controller.UpdateWorkteam(workteam1, string.Empty);
        }

        [TestMethod]
        public void CangeToSameName()
        {
            controller.UpdateWorkteam(workteam1, workteam1.Foreman);
        }
    }
}
