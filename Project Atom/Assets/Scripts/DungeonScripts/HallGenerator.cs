using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallGenerator : Module
{

  void Awake()
  {
    collisionPosition = colliderTransform.position;
    exitTag = MyTags.EXIT_HALL_TAG;
  }

  public void Autodestroy()
  {
    Destroy(this.gameObject);
  }

}//class
