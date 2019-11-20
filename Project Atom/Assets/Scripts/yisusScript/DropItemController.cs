using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : MonoBehaviour
{
    public List<GameObject> dropableItems;
    public float numItems;
    public List<GameObject> itemsPull;
   
    void Start()
    {
        CreatePullOfItems();
    }

    //Create Pull of items
    void CreatePullOfItems()
    {
        itemsPull = new List<GameObject>();
        for (int i = 0; i < dropableItems.Count; i++)
        {
            for (int j = 0; j < numItems; j++)
            {
                GameObject item = Instantiate(dropableItems[i], transform);
                item.SetActive(false);
                itemsPull.Add(item);
            }
        }
    }
}

