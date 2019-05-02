using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application_layer;
using Domain;
using System.Collections.Generic;
using Application_layer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class DeleteWorkteam
    {
        //[TestMethod]
        //public void TestSuccesfulDeletion()
        //{
        //    Controller controller = Controller.Instance;
        //    string foreman = "Hans";
        //    controller.CreateWorkteam(foreman);

        //    Workteam workteam = controller.GetWorkteamByName(foreman);
        //    controller.DeleteWorkteam(workteam);
        //    controller.GetWorkteamByName(foreman);
        //    Assert.AreEqual(, controller.GetAllWorkteams());
        //}

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ExpectedExceptionDeleteNonExistentWorkteam()
        {
            Controller controller = Controller.Instance;
            string foreman = "Hans";

            Workteam workteam = new Workteam(foreman);


            controller.DeleteWorkteam(workteam);
        }
    }
}
