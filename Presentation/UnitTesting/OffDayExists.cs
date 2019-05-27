using System;
using Application_layer;
using Domain;
using Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{

    [TestClass]
    public class OffDayExists
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void OffDayDoesExist()
        {
            Workteam workteam = controller.CreateWorkteam("OffDayDoesExist");
            Offday offday = controller.CreateOffday(workteam, OffdayReason.Helligdag, DateTime.Today, 5);
            Assert.AreEqual(true, controller.OffdayExists(offday));
        }

        [TestMethod]
        public void OffDayDoesntExists()
        {
            Workteam workteam = controller.CreateWorkteam("OffDayDoesntExist");
            Offday offday = controller.CreateOffday(workteam, OffdayReason.Helligdag, DateTime.Today, 5);
            controller.DeleteOffday(workteam, offday);
            Assert.AreEqual(false, controller.OffdayExists(offday));
        }
    }
}
