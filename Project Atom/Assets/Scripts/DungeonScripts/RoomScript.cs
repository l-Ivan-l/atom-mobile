using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : Module
{
    public int nWalls;
    //public int nDoors;
    public GameObject[] connectors;
    public GameObject[] placeHolders;
    public GameObject wallModule;
    public GameObject doorModule;
    public int[] connectorsIndex;
    private bool[] isConnection;
    private int doorIndex;
    private List<GameObject> doorsInRoom = new List<GameObject>();
    private bool havePortal;
    public GameObject portalPoint;

    public List<GameObject> DoorsInRoom {
      get {return doorsInRoom;}
      set {doorsInRoom = value;}
    }

    public bool HavePortal {
      get {return havePortal;}
      set {havePortal = value;}
    }

    void Awake()
    {
      collisionPosition = colliderTransform.position;
      exitTag = MyTags.EXIT_ROOM_TAG;
    }

    void Start()
    {
      doorIndex = 0;
      havePortal = false;
      isConnection = new bool[connectors.Length];
      for(int i = 0; i < connectors.Length; i++) {
        isConnection[i] = true;
      }
    }

    void GetWall(int _wallIndex)
    {

      if(connectorsIndex[_wallIndex] == 1) {
        if(!isConnection[doorIndex]) {
          GameObject doorObj = (GameObject)Instantiate(doorModule, placeHolders[_wallIndex].transform.position,
          placeHolders[_wallIndex].transform.rotation, this.gameObject.transform);
          doorsInRoom.Add(doorObj);
        } else {
          GameObject wallObj = (GameObject)Instantiate(wallModule, placeHolders[_wallIndex].transform.position,
          placeHolders[_wallIndex].transform.rotation, this.gameObject.transform);
        }
        doorIndex++;
      } else {
        GameObject wallObj = (GameObject)Instantiate(wallModule, placeHolders[_wallIndex].transform.position,
        placeHolders[_wallIndex].transform.rotation, this.gameObject.transform);
      }

      placeHolders[_wallIndex].SetActive(false);
    }

    public void GenerateWalls()
    {
      for(int i = 0; i < nWalls; i++) {
        GetWall(i);
      }
    }

    public void EvaluateConnections()
    {
      for(int i = 0; i < connectors.Length; i++) {
        isConnection[i] = connectors[i].gameObject.activeInHierarchy;
      }
    }

}//class




























//space
