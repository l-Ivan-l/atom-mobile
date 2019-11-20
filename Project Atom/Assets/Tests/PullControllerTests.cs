using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PullControllerTests : MonoBehaviour
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PullController_CreatePull_PullLenghtEqualToNumOfElements() {
            //Arrange
            PullController pullController = new PullController();
            List<GameObject> pullList = new List<GameObject>();
            GameObject _object = new GameObject();
            int numOfElements = 15;
            //Act
            pullController.CreatePull(pullList, _object, numOfElements);
            //Assert
            Assert.AreEqual(numOfElements, pullList.Count);
            

        }
    }
}
