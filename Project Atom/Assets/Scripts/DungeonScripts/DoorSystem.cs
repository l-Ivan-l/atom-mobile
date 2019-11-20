using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
  private Collider[] doorSensors;
  private RoomScript room;
  private EnemiesGenerator enemiesGen;
  private Minimap minimapScript;
  private bool doorsClosed;
  // Start is called before the first frame update
  void Start()
  {
    room = this.gameObject.transform.parent.gameObject.GetComponent<RoomScript>();
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
    if(target.gameObject.CompareTag(MyTags.PLAYER_TAG)) {
      //Minimap = GameObject.Find("Minimap_Btn");
      minimapScript = GameObject.Find("MinimapCamera").GetComponent<Minimap>();
      minimapScript.CanInteract = false;
      //Minimap.SetActive(false);
      //Freeze Player
      Singleton.Instance.StopTime = 0.5f;
      //Close Doors
      for(int i = 0; i < room.DoorsInRoom.Count; i++) {
        room.DoorsInRoom[i].transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("CloseDoor");
      }
      //Desactivate sensors
      for (int i = 0; i < doorSensors.Length; i++) {
        doorSensors[i].enabled = false;
      }
      doorsClosed = true;
      ActivateEnemies();
    }
  }

  public void OpenDoors()
  {
    for(int i = 0; i < room.DoorsInRoom.Count; i++) {
      room.DoorsInRoom[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("OpenDoor");
    }
    doorsClosed = false;
    if(this.transform.parent.gameObject.GetComponent<RoomScript>().HavePortal) {
      this.transform.parent.GetChild(this.transform.parent.childCount - 1).gameObject.SetActive(true);
      Debug.Log("Portal Activated");
    }
    //Minimap.SetActive(true);}
    minimapScript.CanInteract = true;
    this.gameObject.SetActive(false);
  }

  public void ActivateEnemies()
  {
    for(int i = 0; i < enemiesGen.EnemiesInRoom.Count; i++) {
      enemiesGen.EnemiesInRoom[i].SetActive(true);
    }
  }

}//class
