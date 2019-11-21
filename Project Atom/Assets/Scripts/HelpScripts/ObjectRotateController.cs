using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotateController
{
  ObjectRotate objectRotate = new ObjectRotate();
  public void RotateObject(float velocity){
    if (velocity < 0) {
        objectRotate.RotateObjectLeft(velocity);
    }
    else if (velocity > 0) {
        objectRotate.RotateObjectRight(velocity);
    }
  }
}
