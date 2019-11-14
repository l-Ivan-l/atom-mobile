using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Module
{
    void Awake()
    {
      collisionPosition = colliderTransform.position;
      exitTag = MyTags.EXIT_ROOM_TAG;
    }

}//class













//space
