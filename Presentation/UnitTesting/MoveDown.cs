using System;
using Application_layer;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class MoveDown
    {
        Controller controller;
        [TestInitialize]
        public void TestInitialize()
        {
            controller = Controller.Instance;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveUnderMin()
        {
            Workteam workteam = controller.CreateWorkteam("MoveUnderMin");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderDown(workteam, order);
        }

        [TestMethod]
        public void SuccesfulMoveDown()
        {
            Workteam workteam = controller.CreateWorkteam("SuccesfulMoveDown");
            Order order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Order order2 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);

            controller.MoveOrderDown(workteam, order1);

            Assert.AreEqual(order1, workteam.orders[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveOrderNotInWorkteam()
        {
            Workteam workteam1 = controller.CreateWorkteam("MoveOrderNotInWorkteam1");
            Workteam workteam2 = controller.CreateWorkteam("MoveOrderNotInWorkteam2");
            Order order = controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderDown(workteam1, order);
        }
    }
}
