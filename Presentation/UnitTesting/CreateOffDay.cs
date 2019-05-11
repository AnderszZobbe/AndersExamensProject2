using System;
using Application_layer;
using Application_layer.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateOffday
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void ReturnOffDay()
        {
            Workteam workteam = controller.CreateWorkteam("ReturnOffDay");

            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, 1);

            Assert.AreEqual(true, workteam.IsAnOffday(DateTime.Today));
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Negativduration()
        {
            Workteam workteam = controller.CreateWorkteam("Negativduration");
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, -1);
        }

        [TestMethod]
        public void TimeLeapYear()
        {
            DateTime date1 = new DateTime(2020, 2, 29, 8, 30, 52);

            Workteam workteam = controller.CreateWorkteam("TimeLeapYear");
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, date1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverlapException))]
        public void ExpectExceptionOverlappingOffDay()
        {
            
            string foreman = "Dennis";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, 1);
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, 1);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyWorkteam()
        {
            controller.CreateOffday(null, OffdayReason.Fredagsfri, DateTime.Today, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExpectExceptionEmptyDuration()
        {
            Workteam workteam = controller.CreateWorkteam("Eric");

            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.Today, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeMax()
        {
            string foreman = "Frederick";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.MaxValue, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeMin()
        {
            string foreman = "Gert";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(workteam, OffdayReason.Fredagsfri, DateTime.MinValue, 2);
        }

       
    }
}
