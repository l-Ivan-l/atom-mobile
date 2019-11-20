using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour
{
    public void AddElementToThePull(List<GameObject> pullList, GameObject _Object)
    {
        GameObject newElement = Instantiate(_Object);
        pullList.Add(newElement);
        Debug.Log("AddElement");
    }
}
