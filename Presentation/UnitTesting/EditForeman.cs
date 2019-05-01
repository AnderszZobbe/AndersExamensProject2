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
        static Controller controller = Controller.Instance;
        Workteam workteam = controller.CreateWorkteam("Hans");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectExceptionChangeNameToNull()
        {
            controller.EditForeman(null, workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateObjectException))]
        public void ExpectExceptionChangeNameToAlreadyExsistingName()
        {
            Workteam workteam2 = controller.CreateWorkteam("Jan");

            controller.EditForeman("Jan", workteam);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectExceptionChangeNameToEmptyString()
        {
            controller.EditForeman("", workteam);
        }
    }
}
