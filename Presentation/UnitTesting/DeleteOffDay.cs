using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Domain.Exceptions;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class DeleteOffDay
    {
        Controller controller;
        Workteam workteam;
        Offday offday1, offday2, offday3;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("DeleteOffDay");
            offday1 = controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, 0);
            offday2 = controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today.AddDays(1), 0);
            offday3 = controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today.AddDays(2), 0);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            controller.DeleteWorkteam(workteam);
        }

        [TestMethod]
        public void CanDelete()
        {
            controller.DeleteOffday(workteam, offday1);
            controller.DeleteOffday(workteam, offday2);
            controller.DeleteOffday(workteam, offday3);
        }

        [TestMethod]
        public void ReturnTrue()
        {
            Assert.IsTrue(controller.DeleteOffday(workteam, offday1));
        }

        [TestMethod]
        public void ReturnFalse()
        {
            Assert.IsFalse(controller.DeleteOffday(workteam, null));
        }

        [TestMethod]
        public void SuccesfulDeletionCount()
        {
            controller.DeleteOffday(workteam, offday2);
            Assert.AreEqual(2, controller.GetAllOffdaysFromWorkteam(workteam).Count);
        }

        [TestMethod]
        public void SuccesfulDeletion()
        {
            controller.DeleteOffday(workteam, offday1);
            Assert.AreEqual(offday2, controller.GetAllOffdaysFromWorkteam(workteam)[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedExceptionDeleteNull()
        {
            controller.DeleteOffday(null, offday1);
        }
    }
}
