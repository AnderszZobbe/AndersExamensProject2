using System;
using Application_layer;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class MoveDown
    {
        Controller controller;
        Workteam workteam;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager();
            Manager.DataProvider = new TestDataProvider();
            controller = Controller.Instance;
            workteam = controller.CreateWorkteam("Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveUnderMin()
        {
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderDown(workteam, order);
        }

        [TestMethod]
        public void SuccesfulMoveDown()
        {
            Order order1 = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);

            controller.MoveOrderDown(workteam, order1);

            Assert.AreEqual(order1, controller.GetAllOrdersFromWorkteam(workteam)[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveOrderNotInWorkteam()
        {
            Workteam workteam2 = controller.CreateWorkteam("Test2");
            Order order = controller.CreateOrder(workteam2, null, null, null, null, null, null, null, null, null, null, null);
            controller.MoveOrderDown(workteam, order);
        }
    }
}
