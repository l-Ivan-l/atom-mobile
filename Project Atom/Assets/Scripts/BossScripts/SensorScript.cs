using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public Animator Door;
    public string thisRoomTag;
    //private GameObject miniMap;
    private Minimap minimapScript;

    void OnTriggerEnter(Collider target)
    {
      LockRoom(target.gameObject.tag, thisRoomTag);
    }

    public void LockRoom(string _targetTag, string _roomTag)
    {
      if(_targetTag == MyTags.PLAYER_TAG && _roomTag == MyTags.BOSS_ROOM_TAG) {
        //miniMap = GameObject.Find("Minimap_Btn");
        //miniMap.SetActive(false);
        minimapScript = GameObject.Find("MinimapCamera").GetComponent<Minimap>();
        minimapScript.CanInteract = false;
        this.gameObject.transform.parent.gameObject.GetComponent<PCBoss>().PlayerInRoom = true;
        //Freeze Player
        Singleton.Instance.StopTime = 5f;
        //Close Door
        Door.SetTrigger("CloseDoor");
        this.gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("PLAYER ENTER BOSS ROOM");
      } else if(_targetTag == MyTags.PLAYER_TAG && _roomTag == MyTags.TREASURE_ROOM_TAG) {
        //Freeze Player
        Singleton.Instance.StopTime = 0.5f;
        //Close Door
        Door.SetTrigger("CloseDoor");
        this.gameObject.GetComponent<Collider>().enabled = false;
      }
    }

}//class
