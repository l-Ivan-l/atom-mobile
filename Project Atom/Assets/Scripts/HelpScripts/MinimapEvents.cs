using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MinimapEvents : EventTrigger
{
  private Animator minimapAnim;
  private Vector3 firstTouch;
  private Vector3 lastTouch;

  private bool minimapIsVisible;

  public bool MiniMapIsVisible {
    get {return minimapIsVisible;}
    set {minimapIsVisible = value;}
  }

  void Awake()
  {
    minimapAnim = this.GetComponent<Animator>();
  }

  public override void OnPointerDown(PointerEventData data)
  {
      firstTouch = Input.mousePosition;
  }

  public override void OnDrag(PointerEventData data)
  {
      lastTouch = Input.mousePosition;
      Debug.Log("MINIMAP DRAG" + Input.mousePosition);
  }

  public override void OnPointerUp(PointerEventData data)
  {
      if(lastTouch.y > firstTouch.y) {
        Debug.Log("MINIMAP UP" + Input.mousePosition);
        minimapAnim.SetTrigger("MinimapUp");
        minimapIsVisible = false;
      } else if(lastTouch.y < firstTouch.y) {
        Debug.Log("MINIMAP DOWN" + Input.mousePosition);
        minimapAnim.SetTrigger("MinimapDown");
        minimapIsVisible = true;
      }
  }

}//class
