using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CameraZoomTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CameraZoom_CameraZoomOut_AssignHeightToMaxHeightAndAssignDistanceToMaxDistance()
        {
            //Arrange
            var cameraZoom = new CameraZoom();
            cameraZoom.height = 7.5f;
            cameraZoom.distance = 7.5f;
            cameraZoom.maxDistance = 1f;
            cameraZoom.maxHeight = cameraZoom.height + 1f;
            //Act
            cameraZoom.CameraZoomOut();
            //Assert
            Assert.AreEqual(cameraZoom.height, cameraZoom.maxHeight);
            Assert.AreEqual(cameraZoom.distance, cameraZoom.maxDistance);
        }

        [Test]
        public void CameraZoom_CameraZoomIn_AssignHeightToMinHeightAndAssignDistanceToMinDistance()
        {
            //Arrange
            var cameraZoom = new CameraZoom();
            cameraZoom.height = 7.5f;
            cameraZoom.distance = 7.5f;
            cameraZoom.minDistance = cameraZoom.distance;
            cameraZoom.minHeight = cameraZoom.height;
            //Act
            cameraZoom.CameraZoomIn();
            //Assert
            Assert.AreEqual(cameraZoom.height, cameraZoom.minHeight);
            Assert.AreEqual(cameraZoom.distance, cameraZoom.minDistance);
        }
    }
}
