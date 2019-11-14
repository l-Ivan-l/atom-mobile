using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoomScript : MonoBehaviour
{
    private Vector3 touchStart;
    private Vector3 firstTouch;
    private Vector3 currentTouch;
    private float zoomOutMin;
    private float zoomOutMax;

    private bool minimapIsActive;
    private float limitX = 75f;
    private float limitZ = 75f;
    private Camera thisCamera;
    private bool paning;
    private Minimap minimap;

    public bool Paning {
      get {return paning;}
      set {paning = value;}
    }

    void Awake()
    {
      thisCamera = this.GetComponent<Camera>();
      minimap = this.GetComponent<Minimap>();
    }

    // Start is called before the first frame update
    void Start()
    {
        paning = false;
        zoomOutMin = 5f;
        zoomOutMax = 24f;
    }

    // Update is called once per frame
    void Update()
    {
      minimapIsActive = GetComponent<Minimap>().MiniMapMode;
      if(minimapIsActive) {
        Pan();
        Debug.Log("Minimap Mode On");
      }
    }

    void Pan()
    {
      if(Input.GetMouseButtonDown(0)) {
        Debug.Log("TouchStart");
        touchStart = thisCamera.ScreenToWorldPoint(Input.mousePosition);
        firstTouch = Input.mousePosition;
      }
      if(Input.touchCount == 2) {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
        float difference = currentMagnitude - prevMagnitude;
        Zoom(difference * 0.01f);
      } else if(Input.GetMouseButton(0)) {
        Debug.Log("TouchSlide");
        Vector3 direction = touchStart - thisCamera.ScreenToWorldPoint(Input.mousePosition);
        //limitX = 75, limitZ = 50
        this.transform.position += direction;
        Vector3 newPos = this.transform.position;
        newPos.x = Mathf.Clamp(this.transform.position.x, -limitX, limitX);
        newPos.z = Mathf.Clamp(this.transform.position.z, -limitZ, limitZ);
        this.transform.position = newPos;
        currentTouch = Input.mousePosition;
      } else if(Input.GetMouseButtonUp(0)) {
        if(firstTouch == currentTouch) {
          minimap.MinimapModeOff();
        }
      }
    }

    void Zoom(float _increment)
    {
      thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize - _increment, zoomOutMin, zoomOutMax);
    }

}//class
