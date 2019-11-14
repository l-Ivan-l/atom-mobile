using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsMove : MonoBehaviour
{
    public float speedR;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up*speedR*Time.deltaTime);
    }
}
