using System;
using Application_layer;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class MoveUp
    {
        Controller controller;
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveOverMax()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderUp(workteam, order);
        }

        [TestMethod]
        public void SuccesfulMoveUp()
        {
            Order order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Order order2 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);

            controller.MoveOrderUp(workteam, order2);

            Assert.AreEqual(order2, workteam.orders[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveOrderNotInWorkteam()
        {
            Workteam workteam2 = controller.CreateWorkteam("MoveOrderNotInWorkteam");
            Order order = controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderUp(workteam, order);
        }
    }
}
