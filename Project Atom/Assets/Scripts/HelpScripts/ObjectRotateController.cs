using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotateController
{
  ObjectRotate objectRotate = new ObjectRotate();
  public void RotateObject(float velocity){
    if (velocity < 0) {
        objectRotate.GetComponent<IRotate>().RotateObjectLeft(velocity);
    }
    else if (velocity > 0) {
        objectRotate.GetComponent<IRotate>().RotateObjectRight(velocity);
    }
  }
}
