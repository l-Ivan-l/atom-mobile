using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ObjectsMoveTests: MonoBehaviour , IRotate
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ObjectsMove_RotateObject_EachFrameObjectRotatesLeft()
        {
            //Arrange

            ObjectRotateController objetRotateControler = new ObjectRotateController();
            float velocity = 12f;


            //Act
            objetRotateControler.RotateObject(velocity);
          //Assert
          Assert.IsTrue(left);
          Assert.IsFalse(right);
        }
        public void ObjectsMove_RotateObject_EachFrameObjectRotatesRight()
        {
            //Arrange

            ObjectRotateController objetRotateControler = new ObjectRotateController();
            float velocity = -12f;


            //Act
            objetRotateControler.RotateObject(velocity);
          //Assert
            Assert.IsFalse(left);
            Assert.IsTrue(right);
        }
        bool left;
        void IRotate.RotateObjectLeft(float Velocity)
        {
            left = true;
            right = false;
        }
        bool right;
        void IRotate.RotateObjectRight(float Velocity)
        {
            left = false;
            right = true;
        }
    }
}
