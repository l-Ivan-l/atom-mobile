using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ObjectsMoveTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ObjectsMove_RotateObject_EachFrameObjectRotatesClockWise()
        {
          //Arrange

          var ObjectRotateController = new ObjectRotateController();


          //Act
          ObjectMove.RotateObject();
          //Assert
          Assert.AreApproximatelyEqual( 50, transform.Rotate);
        }
    }
}
