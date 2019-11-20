using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom
{
  public float minHeight;
  public float maxHeight;
  public float minDistance;
  public float maxDistance;

  public float height;
  public float distance;

  public void CameraZoomOut()
  {
    height = maxHeight;
    distance = maxDistance;
  }

  public void CameraZoomIn()
  {
    height = minHeight;
    distance = minDistance;
  }
}
