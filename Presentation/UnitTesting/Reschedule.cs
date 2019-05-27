using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class Reschedule
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void RescheduleWorks()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule1");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
           controller.Reschedule(workteam,order,DateTime.Today);
        }

        [TestMethod]
        public void RescheduleAfterNewAssignment()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule2");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            controller.Reschedule(workteam, order, DateTime.Today);
        }

        [TestMethod]
        public void RescheduleAfterTwoOrders()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule3");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            Order orderTwo = controller.CreateOrder(workteam, 2, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(orderTwo, Workform.Dagsarbejde, 5);
            controller.Reschedule(workteam, order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(6), orderTwo.StartDate);
        }

        [TestMethod]
        public void RescheduleAfterTwoOrdersWithNoAssignments()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule4");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            Order orderTwo = controller.CreateOrder(workteam, 2, "", "", null, null, "", null, DateTime.Today, "", "", "");
            Order orderThree = controller.CreateOrder(workteam, 3, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.Reschedule(workteam, order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(6), orderTwo.StartDate);
            Assert.AreEqual(DateTime.Today.AddDays(6), orderThree.StartDate);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongWorkTeam()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule5");
            Workteam workteamTwo = controller.CreateWorkteam("Reschedule6");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            controller.Reschedule(workteamTwo, order, DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullWorkTeam()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule7");
            
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            controller.Reschedule(null, order, DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullOrder()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule8");

            controller.Reschedule(workteam, null, DateTime.Today);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MaxTime()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule9");

            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            controller.Reschedule(workteam, order, DateTime.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MinTime()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule9");

            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
            controller.CreateAssignment(order, Workform.Dagsarbejde, 5);
            controller.Reschedule(workteam, order, DateTime.MinValue.AddDays(-1));
        }
    }
}
