using System;
using Application_layer;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class MoveUp
    {
        Controller controller;
        [TestInitialize]
        public void TestInitialize()
        {
            controller = Controller.Instance;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveOverMax()
        {
            Workteam workteam = controller.CreateWorkteam("MoveUp1");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderUp(workteam, order);
        }

        [TestMethod]
        public void SuccesfulMoveUp()
        {
            Workteam workteam = controller.CreateWorkteam("MoveUp2");
            Order order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Order order2 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);

            controller.MoveOrderUp(workteam, order2);

            Assert.AreEqual(order2, workteam.orders[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveOrderNotInWorkteam()
        {
            Workteam workteam1 = controller.CreateWorkteam("MoveUp3");
            Workteam workteam2 = controller.CreateWorkteam("MoveUp4");
            Order order = controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderUp(workteam1, order);
        }
    }
}
