using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BossTest
    {
        [Test]
        public void BossTest_PassToPhaseTwoWhenTheLifeIsAtFiftyPercent()
        {
            //Arrange...
            var mockBoss = new Boss();

            //Act...
            mockBoss.BossPhaseState(50f, 50f, 25f);

            //Assert...
            Assert.AreEqual(2, mockBoss.bossPhase);
        }

        [Test]
        public void BossTest_PassToPhaseThreeWhenTheLifeIsAtTwentyFivePercent()
        {
            //Arrange...
            var mockBoss = new Boss();

            //Act...
            mockBoss.BossPhaseState(25f, 50f, 25f);

            //Assert...
            Assert.AreEqual(3, mockBoss.bossPhase);
        }

        [Test]
        public void BossTest_PassToPhaseFourWhenTheLifeIsZero()
        {
            //Arrange...
            var mockBoss = new Boss();

            //Act...
            mockBoss.BossPhaseState(0f, 50f, 25f);

            //Assert...
            Assert.AreEqual(4, mockBoss.bossPhase);
        }
    }
}
