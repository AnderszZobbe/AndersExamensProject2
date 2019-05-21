using System;
using Application_layer;
using Domain.Exceptions;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace UnitTesting
{
    [TestClass]
    public class CreateAssignment
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
        public void AssignmentIsMade()
        {
            Workteam workteam = controller.CreateWorkteam("AssignmentIsMade");
            Order order = controller.CreateOrder(workteam,null,null,null,null,null,null,null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, Workform.Dag, 0);


            Assert.IsNotNull(assignment);
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativDuration()
        {
            Workteam workteam = controller.CreateWorkteam("NegativDuration");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dag, -1);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooLongDuration()
        {
            Workteam workteam = controller.CreateWorkteam("TooLongDuration");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dag, int.MaxValue);

        }
        [TestMethod]
        public void CorrectDurationIsSaved()
        {
            Workteam workteam = controller.CreateWorkteam("CorrectDurationIsSaved");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, Workform.Dag, 1);

            Assert.AreEqual(1, assignment.Duration);
            assignment = controller.CreateAssignment(order, Workform.Dag, 0);
            Assert.AreEqual(0, assignment.Duration);
            assignment = controller.CreateAssignment(order, Workform.Dag, 10);
            Assert.AreEqual(10, assignment.Duration);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionNullOrder()
        {
            controller.CreateAssignment(null, Workform.Dag, 0);
        }

        [TestMethod]
        public void CorrectWorkforeIsSaved()
        {
            Workteam workteam = controller.CreateWorkteam("CorrectWorkforeIsSaved");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, Workform.Dag, 0);

            Assert.AreEqual(Workform.Dag, assignment.Workform);
            assignment = controller.CreateAssignment(order, Workform.Nat, 0);
            Assert.AreEqual(Workform.Nat, assignment.Workform);
            assignment = controller.CreateAssignment(order, Workform.Flytning, 0);
            Assert.AreEqual(Workform.Flytning, assignment.Workform);
        }

        //IndexOutOfRangeException might not be correct
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderDoesNotExist()
        {
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            controller.CreateAssignment(order, Workform.Dag, 0);

        }
    }
}
