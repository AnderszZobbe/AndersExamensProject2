using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class GetNextAvailableDate
    {
        Controller controller;
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("GetNextAvailableDate");
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EmptyOrder()
        {
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            workteam.GetNextAvailableDate(order);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AnotherWorkteamsOrder()
        {
            Workteam workteamTwo = controller.CreateWorkteam("GetNextAvailableDate3");
            Order order = controller.CreateOrder(workteamTwo, null, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            workteam.GetNextAvailableDate(order);
        }
        [TestMethod]
        public void CorrectDateWithOneAssignment()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(2), workteam.GetNextAvailableDate(order));
        }
        [TestMethod]
        public void CorrectDateWithTwoAssignment()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            controller.CreateAssignment(order, Workform.Nattearbejde, 0);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(4), workteam.GetNextAvailableDate(order));
        }
        [TestMethod]
        public void CorrectDateWithNoAssignment()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(0), workteam.GetNextAvailableDate(order));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidStartDate()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 0);
            order.StartDate = DateTime.MaxValue;
            workteam.GetNextAvailableDate(order);
        }
    }
}
