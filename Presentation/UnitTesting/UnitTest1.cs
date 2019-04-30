using System;
using Application_layer;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        Controller controller;

        [TestInitialize]
        public void TestInitialize()
        {
            controller = Controller.Instance;
        }

        [TestMethod]
        public void ReturnWorkteam()
        {
            string foreman = "Alpha";

            Workteam workteam = controller.CreateWorkteam(foreman);

            Assert.IsNotNull(workteam);
        }

        [TestMethod]
        public void EstablishMoreWorkteam()
        {
            string foreman = "Bravo";

            Workteam workteam = controller.CreateWorkteam(foreman);

            Workteam workteamFound = controller.GetAllWorkteams().Find(o => o.Foreman == workteam.Foreman);

            Assert.AreEqual(workteam, workteamFound);
        }
    }
}
