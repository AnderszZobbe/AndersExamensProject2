using System;
using Application_layer;
using Application_layer.Exceptions;
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
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void RescheduleWorks()
        {
            Workteam workteam = controller.CreateWorkteam("Reschedule1");
            Order order = controller.CreateOrder(workteam, 1, "", "", null, null, "", null, DateTime.Today, "", "", "");
           controller.Reschedule(workteam,order,DateTime.Today);
        }
        
    }
}
