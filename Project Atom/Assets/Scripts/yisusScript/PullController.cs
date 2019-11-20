using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullController 
{
    Pull Pull = new Pull();
   

    public void CreatePull(List<GameObject> pullList, GameObject _object, int numObjects )
    {

        for (int j = 0; j < numObjects; j++)
        {
            Debug.Log("AddElemten " + j);
            Pull.AddElementToThePull(pullList, _object);
            
        }
    }
}
