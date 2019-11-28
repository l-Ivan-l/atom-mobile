using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossRoom : Module
{
    public GameObject topConnection;
    public GameObject bottomConnection;
    public GameObject portal;

    void Awake()
    {
      collisionPosition = colliderTransform.position;
      exitTag = MyTags.EXIT_ROOM_TAG;
    }

    void OnTriggerEnter(Collider target)
    {
      if(target.gameObject.CompareTag(MyTags.PLAYER_TAG)) {
        ModuleDiscovered();
        portal.SetActive(true);
      }
    }

}//class
