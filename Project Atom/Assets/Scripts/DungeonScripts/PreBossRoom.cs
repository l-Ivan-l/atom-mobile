using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossRoom : Module
{
    public GameObject topConnection;
    public GameObject bottomConnection;

    void Awake()
    {
      collisionPosition = colliderTransform.position;
      exitTag = MyTags.EXIT_ROOM_TAG;
    }

}//class
