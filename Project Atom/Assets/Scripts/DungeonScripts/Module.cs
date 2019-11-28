using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
  public Vector3 collisionSize;
  protected Vector3 collisionPosition;
  public Transform colliderTransform;
  public GameObject[] minimapIcons;

  protected string exitTag;

  public Vector3 GetCollisionPosition()
  {
    collisionPosition = colliderTransform.position;
    return collisionPosition;
  }

  public List<GameObject> GetExits()
  {
    var exits = new List<GameObject>();
    int i = 0;
    while(this.transform.GetChild(i).transform.gameObject.CompareTag(exitTag)) {
      exits.Add(this.transform.GetChild(i).transform.gameObject);
      i++;
    }
    return exits;
  }

  public void ModuleDiscovered()
  {
    for(int i = 0; i < minimapIcons.Length; i++)
    {
      minimapIcons[i].SetActive(true);
    }
  }

  public void ActivateModule()
  {
    for(int i = 0; i < this.transform.childCount; i++)
    {
      this.transform.GetChild(i).gameObject.SetActive(true);
    }
  }

  public void DeactivateModule()
  {
    for(int i = 0; i < this.transform.childCount; i++)
    {
      this.transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void OnDrawGizmosSelected()
  {
    collisionPosition = colliderTransform.position;
    // Display the explosion radius when selected
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(collisionPosition, collisionSize);
    //Gizmos.DrawWireSphere(transform.position - collisionCenter, collisionSize);
  }

  void OnTriggerEnter(Collider target)
  {
    if(target.CompareTag(MyTags.PLAYER_TAG)) {
      ModuleDiscovered();
    }
  }

}//class
