using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
  private Collider[] doorSensors;
  private RoomScript room;
  private TreasureRoom treasureRoom;
  private EnemiesGenerator enemiesGen;
  private bool doorsClosed;
  private GameObject Minimap;
  private Minimap minimapScript;

  public Animator doorAnim;

  void Start()
  {
    enemiesGen = this.gameObject.transform.parent.gameObject.GetComponent<EnemiesGenerator>();
    doorSensors = GetComponents<Collider>();
    doorsClosed = false;
  }

  void LateUpdate()
  {
    if(enemiesGen.EnemiesInRoom.Count == 0 && doorsClosed) {
      OpenDoors();
    }
  }

  void OnTriggerEnter(Collider target)
  {
    if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.ROOM_TAG)) {
      if(target.gameObject.CompareTag(MyTags.PLAYER_TAG)) {
        room = this.gameObject.transform.parent.gameObject.GetComponent<RoomScript>();
        Minimap = GameObject.Find("Minimap_Btn");
        minimapScript = GameObject.Find("MinimapCamera").GetComponent<Minimap>();
        minimapScript.CanInteract = false;
        Minimap.GetComponent<Animator>().SetTrigger("MinimapUp");
        //Freeze Player
        Singleton.Instance.StopTime = 0.5f;
        //Close Doors
        for(int i = 0; i < room.DoorsInRoom.Count; i++) {
          room.DoorsInRoom[i].transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("CloseDoor");
          room.DoorsInRoom[i].transform.GetChild(1).gameObject.SetActive(true);
        }
        //Desactivate sensors
        for (int i = 0; i < doorSensors.Length; i++) {
          doorSensors[i].enabled = false;
        }
        doorsClosed = true;
        ActivateEnemies();
      }
    } else if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.TREASURE_ROOM_TAG)) {
      if(target.gameObject.CompareTag(MyTags.PLAYER_TAG)) {
        treasureRoom = this.gameObject.transform.parent.gameObject.GetComponent<TreasureRoom>();
        Minimap = GameObject.Find("Minimap_Btn");
        minimapScript = GameObject.Find("MinimapCamera").GetComponent<Minimap>();
        minimapScript.CanInteract = false;
        Minimap.GetComponent<Animator>().SetTrigger("MinimapUp");
        //Freeze Player
        Singleton.Instance.StopTime = 0.5f;
        //Close Doors
        doorAnim.SetTrigger("CloseDoor");
        /*
        for(int i = 0; i < treasureRoom.DoorsInRoom.Count; i++) {
          treasureRoom.DoorsInRoom[i].transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("CloseDoor");
          treasureRoom.DoorsInRoom[i].transform.GetChild(1).gameObject.SetActive(true);
        }
        */
        //Desactivate sensors
        for (int i = 0; i < doorSensors.Length; i++) {
          doorSensors[i].enabled = false;
        }
        doorsClosed = true;
        ActivateEnemies();
      }
    }

  }

  void ActivateEnemies()
  {
    for(int i = 0; i < enemiesGen.EnemiesInRoom.Count; i++) {
      enemiesGen.EnemiesInRoom[i].SetActive(true);
    }
  }

  void OpenDoors()
  {
    if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.ROOM_TAG)) {
      for(int i = 0; i < room.DoorsInRoom.Count; i++) {
        room.DoorsInRoom[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("OpenDoor");
      }
      doorsClosed = false;
      if(this.transform.parent.gameObject.GetComponent<RoomScript>().HavePortal) {
        this.transform.parent.GetChild(this.transform.parent.childCount - 1).gameObject.SetActive(true);
        Debug.Log("Portal Activated");
      }
    } else if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.TREASURE_ROOM_TAG)) {
      doorAnim.SetTrigger("OpenDoor");
    }
    Minimap.GetComponent<Animator>().SetTrigger("MinimapDown");
    minimapScript.CanInteract = true;
    this.gameObject.SetActive(false);
  }

}//class
