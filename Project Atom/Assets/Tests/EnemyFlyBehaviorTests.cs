using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EnemyFlyBehaviorTests
    {
        // A Test behaves as an ordinary method
        [Test]
      public void EnemyFlyBehaviour_Die_EnemyDieWhenLifeIsLessThanZero()
      {
          //Arrange
          var enemyInstance = new EnemyFlyBehavior();
          var enemyMock = new MockFly();
          enemyInstance.FlyEnemy=enemyMock;
         int life = -3;

          //Act
          enemyInstance.Die2(life);

          //Assert
          Assert.AreEqual(true,enemyMock.flyEnemyDead);


      }
    }
}
