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

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EmptyOrder()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate1");
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            workteam.GetNextAvailableDate(order);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AnotherWorkteamsOrder()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate2");
            Workteam workteamTwo = controller.CreateWorkteam("GetNextAvailableDate3");
            Order order = controller.CreateOrder(workteamTwo, null, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            workteam.GetNextAvailableDate(order);
        }
        [TestMethod]
        public void CorrectDateWithOneAssignment()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate4");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(2), workteam.GetNextAvailableDate(order));
        }
        [TestMethod]
        public void CorrectDateWithTwoAssignment()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate5");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            controller.CreateAssignment(order, Workform.Nattearbejde, 0);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(3), workteam.GetNextAvailableDate(order));
        }
        [TestMethod]
        public void CorrectDateWithNoAssignment()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate6");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.UpdateOrderStartDate(order, DateTime.Today);
            Assert.AreEqual(DateTime.Today.AddDays(0), workteam.GetNextAvailableDate(order));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidStartDate()
        {
            Workteam workteam = controller.CreateWorkteam("GetNextAvailableDate7");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dagsarbejde, 0);
            order.StartDate = DateTime.MaxValue;
            workteam.GetNextAvailableDate(order);
        }
    }
}
