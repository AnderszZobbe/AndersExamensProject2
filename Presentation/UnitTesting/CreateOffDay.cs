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
            string foreman = "Kevin";
            Workteam workteam = controller.CreateWorkteam(foreman);

            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today,1,workteam);

            Assert.AreEqual(true, workteam.IsAnOffday(DateTime.Today));
            
        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void Negativduration()
        {
            string foreman = "Bo";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today, -1, workteam);
        }

        [TestMethod]
        public void TimeLeapYear()
        {
            DateTime date1 = new DateTime(2020, 2, 29, 8, 30, 52);
            string foreman = "cecile";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(OffdayReason.Fredagsfri, date1, 1, workteam);
        }
        [TestMethod]
        [ExpectedException(typeof(OverlapException))]
        public void ExpectExceptionOverlappingOffDay()
        {
            
            string foreman = "Dennis";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today, 1, workteam);
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today, 1, workteam);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyWorkteam()
        {
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today, 1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void ExpectExceptionEmptyDuration()
        {
            Workteam workteam = controller.CreateWorkteam("Eric");

            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.Today, -1, workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void TimeMax()
        {
            string foreman = "Frederick";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.MaxValue, 2, workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void TimeMin()
        {
            string foreman = "Gert";

            Workteam workteam = controller.CreateWorkteam(foreman);
            controller.CreateOffday(OffdayReason.Fredagsfri, DateTime.MinValue, 2, workteam);
        }

       
    }
}
