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
    public class DeleteOffDay
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
            Workteam workteam = controller.CreateWorkteam("DeleteOffDay1");
            Offday offday = controller.CreateOffday(workteam,OffdayReason.Fredagsfri,DateTime.Today,1);
            Assert.IsTrue(controller.DeleteOffday(workteam, offday));
            Assert.AreEqual(0, controller.GetAllOffdaysFromWorkteam(workteam).Count);
          
        }

        [TestMethod]
        public void TestDeletionOfOffdayOnWrongWorkteam()
        {
            Workteam workteam = controller.CreateWorkteam("DeleteDeleteOffDay2");
            Workteam workteamtwo = controller.CreateWorkteam("DeleteDeleteOffDay3");
            Offday offday = controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, 1);
            Assert.IsTrue(controller.DeleteOffday(workteamtwo, offday));

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExpectedExceptionDeleteNull()
        {
            Assert.AreEqual(false, controller.DeleteOffday(null, null));
        }
    }
    }
