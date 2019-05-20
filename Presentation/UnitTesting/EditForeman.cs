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
            workteam1.Foreman = newForeman;

            Assert.AreEqual(newForeman, workteam1.Foreman);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            workteam1.Foreman = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            workteam1.Foreman = workteam2.Foreman;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            workteam1.Foreman = "";
        }

        [TestMethod]
        public void CangeToSameName()
        {
            workteam1.Foreman = workteam1.Foreman;
        }
    }
}
