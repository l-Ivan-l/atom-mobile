using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public LayerMask minimapLayer;
    private bool minimapMode;
    public Camera mainCamera;
    private Camera minimapCamera;
    public GameObject HUD;
    public RenderTexture minimapRenderTxt;
    private PanZoomScript panZoomBehaviour;

    public GameObject playerIcon;
    private GameObject[] portalIcons;

    public GameObject minimap;
    private Animator minimapAnim;

    public MinimapEvents minimapEvents;
    private bool canInteract;

    public bool CanInteract {
      get {return canInteract;}
      set {canInteract = value;}
    }

    public bool MiniMapMode {
      get {return minimapMode;}
      set {minimapMode = value;}
    }

    void Awake()
    {
      minimapCamera = GetComponent<Camera>();
      panZoomBehaviour = GetComponent<PanZoomScript>();
      minimapAnim = minimap.GetComponent<Animator>();
    }

    void Start()
    {
      minimapMode = false;
      canInteract = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      if(!minimapMode) {
        Debug.Log("Minimap Camera Following");
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
      }
    }

    void Update()
    {
      if(minimapMode)
      {
        SelectPortal();
      }
    }

    public void MinimapModeOn()
    {
      if(mainCamera.enabled && minimapEvents.MiniMapIsVisible && canInteract) {
        HUD.SetActive(false);
        minimapCamera.targetTexture = null;
        minimapCamera.enabled = true;
        mainCamera.enabled = false;
        minimapCamera.orthographicSize = 15f;
        minimapMode = true; //true when mninimap zoomed
        panZoomBehaviour.enabled = true;
        GetPortalIcons();
        ExpandIcons();
      }
    }

    public void MinimapModeOff()
    {
      if(minimapCamera.enabled) {
        HUD.SetActive(true);
        mainCamera.enabled = true;
        minimapCamera.enabled = false;
        minimapCamera.enabled = true;
        minimapCamera.targetTexture = minimapRenderTxt;
        minimapCamera.orthographicSize = 12f;
        minimapMode = false;
        panZoomBehaviour.enabled = false;
        ShrinkIcons();
      }
    }

    void SelectPortal()
    {
      if (Input.GetMouseButtonDown(0)){
       RaycastHit hit;
       Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
       if(Physics.Raycast(ray,out hit,100.0f, minimapLayer)) {
         if(hit.transform.gameObject.CompareTag(MyTags.PORTAL_TAG)) {
           Debug.Log("IT WORKSSS!!!");
           hit.transform.parent.gameObject.GetComponent<Portal>().PortalSelected();
           MinimapModeOff();
         }
       }
     }
    }

    void ShrinkIcons()
    {
      for(int i = 0; i < portalIcons.Length; i++) {
        portalIcons[i].transform.localScale = portalIcons[i].transform.localScale * 0.5f;
      }
      playerIcon.transform.localScale = playerIcon.transform.localScale * 2f;
    }

    void ExpandIcons()
    {
      for(int i = 0; i < portalIcons.Length; i++) {
        portalIcons[i].transform.localScale = portalIcons[i].transform.localScale * 2f;
      }
      playerIcon.transform.localScale = playerIcon.transform.localScale * 0.5f;
    }

    public void GetPortalIcons()
    {
      portalIcons = GameObject.FindGameObjectsWithTag(MyTags.PORTAL_TAG);
    }

}//class
