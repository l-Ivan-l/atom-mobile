using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public Animator Door;
    //private GameObject miniMap;
    private Minimap minimapScript;

    void OnTriggerEnter(Collider target)
    {
      if(target.gameObject.CompareTag(MyTags.PLAYER_TAG) && this.gameObject.transform.parent.transform.parent.gameObject.CompareTag(MyTags.BOSS_ROOM_TAG)) {
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
      } else if(target.gameObject.CompareTag(MyTags.PLAYER_TAG) && this.gameObject.transform.parent.gameObject.CompareTag(MyTags.TREASURE_ROOM_TAG)) {
        //Freeze Player
        Singleton.Instance.StopTime = 0.5f;
        //Close Door
        Door.SetTrigger("CloseDoor");
        this.gameObject.GetComponent<Collider>().enabled = false;
      }
    }

}//class
