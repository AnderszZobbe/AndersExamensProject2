﻿using System;
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
            string foreman = "Adam";

            controller.CreateWorkteam(foreman);

            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today,1,workteam);

            Assert.AreEqual(true, workteam.IsAnOffday(DateTime.Today));
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Negativduration()
        {
            string foreman = "Bo";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today, -1, workteam);
        }

        [TestMethod]
        public void TimeLeapYear()
        {
            DateTime date1 = new DateTime(2020, 2, 29, 8, 30, 52);
            string foreman = "cecile";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, date1, 1, workteam);
        }
        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionDuplicateOffDay()
        {
            
            string foreman = "Dennis";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today, 1, workteam);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today, 1, workteam);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionEmptyWorkteam()
        {
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today, 1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionEmptyDuration()
        {
            string foreman = "Eric";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.Today, 0, workteam);
        }

        [TestMethod]
        public void TimeMax()
        {
            string foreman = "Frederick";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.MaxValue, 2, workteam);
        }
        [TestMethod]
        public void TimeMin()
        {
            string foreman = "Gert";

            controller.CreateWorkteam(foreman);
            Workteam workteam = controller.GetWorkteamByName(foreman);
            controller.CreateOffday(OffdayReason.FridayFree, DateTime.MinValue, 2, workteam);
        }

       
    }
}
