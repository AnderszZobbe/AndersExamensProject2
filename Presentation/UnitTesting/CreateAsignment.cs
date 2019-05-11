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
            Order order = controller.CreateOrder(workteam,null,null,null,null,null,null,null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 0, Workform.Dag);


            Assert.IsNotNull(assignment);
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativDuration()
        {
            string foreman = "CreateAssignment2";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, -1, Workform.Dag);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooLongDuration()
        {
            string foreman = "CreateAssignment3";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, int.MaxValue, Workform.Dag);

        }
        [TestMethod]
        public void CorrectDurationIsSaved()
        {
            
            string foreman = "CreateAssignment4";
            Workteam workteam = controller.CreateWorkteam(foreman);
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 1, Workform.Dag);

            Assert.AreEqual(1, assignment.Duration);
            assignment = controller.CreateAssignment(order, 0, Workform.Dag);
            Assert.AreEqual(0, assignment.Duration);
            assignment = controller.CreateAssignment(order, 10, Workform.Dag);
            Assert.AreEqual(10, assignment.Duration);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionNullOrder()
        {
            Assignment assignment = controller.CreateAssignment(null, 0, Workform.Dag);
        }

        [TestMethod]
        public void CorrectWorkforeIsSaved()
        {
            Workteam workteam = controller.CreateWorkteam("CreateAssignment5");
            Order order = controller.CreateOrder(workteam, null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 0, Workform.Dag);

            Assert.AreEqual(Workform.Dag, assignment.Workform);
            assignment = controller.CreateAssignment(order, 0, Workform.Nat);
            Assert.AreEqual(Workform.Nat, assignment.Workform);
            assignment = controller.CreateAssignment(order, 0, Workform.Flytning);
            Assert.AreEqual(Workform.Flytning, assignment.Workform);
        }

        //IndexOutOfRangeException might not be correct
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void OrderDoesNotExist()
        {
            Order order = new Order(null, null, null, null, null, null, null, null, null, null, null);
            Assignment assignment = controller.CreateAssignment(order, 0, Workform.Dag);

        }
    }
}
