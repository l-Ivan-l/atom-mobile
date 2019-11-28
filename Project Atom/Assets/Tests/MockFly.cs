using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MockFly : IFlyEnemy
{
    public bool flyEnemyDead;
    public IEnumerator MorirCo()
    {
        flyEnemyDead=true;
         yield return new WaitForSeconds(1f);
    } 
}
