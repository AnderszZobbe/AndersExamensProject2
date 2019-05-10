using System;
using Application_layer;
using Application_layer.Exceptions;
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
            Controller.Connector = new DBTestConnector();
            controller = Controller.Instance;
        }

        [TestMethod]
        public void AssignmentIsMade()
        {
            string foreman = "CreateAssignment1";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam,null,null,null,null,null,null,null);
            Assignment assignment = controller.CreateAssignment(order, null, null);


            Assert.IsNotNull(assignment);
            
        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void NegativDuration()
        {
            string foreman = "CreateAssignment2";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, -1, null);

        }

        [TestMethod]
        [ExpectedException(typeof(DateOutOfRangeException))]
        public void TooLongDuration()
        {
            string foreman = "CreateAssignment3";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, int.MaxValue, null);

        }
        [TestMethod]
        public void CorrectDurationIsSaved()
        {
            
            string foreman = "CreateAssignment4";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 1, null);

            Assert.AreEqual(1, assignment.Duration);
            assignment = controller.CreateAssignment(order, 0, null);
            Assert.AreEqual(0, assignment.Duration);
            assignment = controller.CreateAssignment(order, 10, null);
            Assert.AreEqual(10, assignment.Duration);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionNullOrder()
        {
            Assignment assignment = controller.CreateAssignment(null, null, null);
        }

        [TestMethod]
        public void CorrectWorkforeIsSaved()
        {
            Workteam workteam = controller.CreateWorkteam("CreateAssignment5");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, null, Workform.Dag);

            Assert.AreEqual(Workform.Dag, assignment.Workform);
            assignment = controller.CreateAssignment(order, null, Workform.Nat);
            Assert.AreEqual(Workform.Nat, assignment.Workform);
            assignment = controller.CreateAssignment(order, null, Workform.Flytning);
            Assert.AreEqual(Workform.Flytning, assignment.Workform);
        }

        //IndexOutOfRangeException might not be correct
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void OrderDoesNotExist()
        {
            Order order = new Order();
            Assignment assignment = controller.CreateAssignment(order, null, null);

        }
    }
}
