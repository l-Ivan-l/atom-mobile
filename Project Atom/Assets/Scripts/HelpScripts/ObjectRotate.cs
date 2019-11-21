using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour , IRotate
{
    public void RotateObjectLeft(float velocity)
    {
      transform.Rotate(Vector3.up*-velocity*Time.deltaTime);
    }
    public void RotateObjectLeft(float velocity)
    {
      transform.Rotate(Vector3.up*velocity*Time.deltaTime);
    }
}
