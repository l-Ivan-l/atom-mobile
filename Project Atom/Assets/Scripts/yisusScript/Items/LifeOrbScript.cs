using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOrbScript : MonoBehaviour,ICollectable
{
    public float lifeAmount;

    void ICollectable.Collect()
    {
        Singleton.Instance.PlayerLife += lifeAmount;
        gameObject.SetActive(false);
    }
}
