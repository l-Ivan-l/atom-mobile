using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
      player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void PortalSelected()
    {
      player.Teletransport(this.transform);
    }
}//class
