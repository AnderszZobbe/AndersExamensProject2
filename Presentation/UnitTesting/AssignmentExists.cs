using System;
using Application_layer;
using Domain;
using Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class AssignmentExists
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
        public void AssignementDoesExist()
        {
            Workteam workteam = controller.CreateWorkteam("AssignementDoesExist");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            Assert.AreEqual(true, controller.AssignmentExists(assignment));
        }

        [TestMethod]
        public void AssignmentDoesntExists()
        {
            Workteam workteam = controller.CreateWorkteam("AssignmentDoesntExists");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, Workform.Dagsarbejde, 1);
            controller.DeleteAssignment(order, assignment);
            Assert.AreEqual(false, controller.AssignmentExists(assignment));

        }
    }
}
