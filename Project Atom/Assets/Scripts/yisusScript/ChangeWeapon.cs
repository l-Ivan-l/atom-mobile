using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public int actualWepon;
    public int actualBullet;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<ShootScript>() != null)
        {
            Singleton.Instance.ActualBullet = actualBullet;
            Singleton.Instance.ActualWeapon = actualWepon;
            other.GetComponentInParent<ShootScript>().ChangeWepon();
            gameObject.SetActive(false);
        }
    }
}
