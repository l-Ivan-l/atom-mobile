using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public Animator Door;
    private GameObject miniMap;
    private Minimap minimapScript;
    public PCBoss pcBoss;
    public MeatBoss meatBoss;

    void OnTriggerEnter(Collider target)
    {
      if(target.gameObject.CompareTag(MyTags.PLAYER_TAG)) {
        miniMap = GameObject.Find("Minimap_Btn");
        miniMap.GetComponent<Animator>().SetTrigger("MinimapUp");
        minimapScript = GameObject.Find("MinimapCamera").GetComponent<Minimap>();

        if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.BOSS_ROOM_TAG)) {
          minimapScript.CanInteract = false;
          pcBoss.PlayerInRoom = true;
          //Freeze Player
          Singleton.Instance.StopTime = 5f;
        } else if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.TREASURE_ROOM_TAG)) {
          //Freeze Player
          Singleton.Instance.StopTime = 0.5f;
        } else if(this.gameObject.transform.parent.gameObject.CompareTag(MyTags.BOSS_02_ROOM_TAG)) {
          minimapScript.CanInteract = false;
          meatBoss.PlayerInRoom = true;
          Singleton.Instance.StopTime = 3f;
        }
        //Close Door
        Door.SetTrigger("CloseDoor");
        this.gameObject.GetComponent<Collider>().enabled = false;
      }
    }

}//class
