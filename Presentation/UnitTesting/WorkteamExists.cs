using System;
using Application_layer;
using Domain;
using Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class WorkteamExists
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Controller.Connector = new Manager(new TestDataProvider());
            controller = Controller.Instance;
        }

        [TestMethod]
        public void WorkteamDoesExist()
        {
            Workteam workteam = controller.CreateWorkteam("WorkteamDoesExists");
            controller.WorkteamExists(workteam);
            Assert.AreEqual(true, controller.WorkteamExists(workteam));
        }

        [TestMethod]
        public void WorkteamDoesntExists()
        {
            Workteam workteam = controller.CreateWorkteam("WorkteamDoesntExists");
            controller.DeleteWorkteam(workteam);
            Assert.AreEqual(false ,controller.WorkteamExists(workteam));
        }
    }
}
