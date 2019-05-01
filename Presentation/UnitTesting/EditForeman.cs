using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Application_layer;
using Application_layer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class EditForeman
    {
        Controller controller = Controller.Instance;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            Workteam workteam = controller.CreateWorkteam("Hans");
            controller.EditForeman(null, workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            Workteam workteam = controller.CreateWorkteam("Tom");
            Workteam workteam2 = controller.CreateWorkteam("Jan");

            controller.EditForeman("Jan", workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            Workteam workteam = controller.CreateWorkteam("Jim");
            controller.EditForeman("", workteam);
        }
    }
}
