using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeaponAtack : MonoBehaviour
{
    public CloseWeapon weapon;
    private float timeBtwAtack;
    public LayerMask Enemies;
    public Joystick joyAtack;
    public Transform hitPos;
    private bool activeCollision;

    //Variables de prueba
    void Start()
    {
        Singleton.Instance.Atack = false;
        activeCollision = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ActiveAtack());
        }
        if(activeCollision && Singleton.Instance.Atack == false)
        {
            Collider[] enemiesToDamage = Physics.OverlapBox(hitPos.position, weapon.atackRange, hitPos.rotation, Enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //enemiesToDamage[i].GetComponent<PathFinding>().life -= weapon.damage;
                    Debug.Log("Taking Damages");
                }
            activeCollision = false;

        }
        //Debug.Log(Singleton.Instance.Atack);
    }

    //void Atacando()
    //{
    //    if (timeBtwAtack <= 0)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {

    //            Collider[] enemiesToDamage = Physics.OverlapBox(hitPos.position, weapon.atackRange, hitPos.rotation, Enemies);
    //            for (int i = 0; i < enemiesToDamage.Length; i++)
    //            {
    //                Debug.Log("Taking Damages");
    //            }
    //        }

    //        timeBtwAtack = weapon.StartTimeBtwAtack;
    //    }
    //    else
    //    {
    //        timeBtwAtack -= Time.deltaTime;
    //    }
    //}


  
    public void ActivarCollision()
    {
        activeCollision = true;
    }

    public void DesactivarCollision()
    {
        activeCollision = false;
    }
    private void OnDrawGizmosSelected()
    {
        if(activeCollision)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitPos.position, weapon.atackRange);
        }
        


    }

    IEnumerator ActiveAtack()
    {
        Singleton.Instance.Atack = true;
        yield return new WaitForSeconds(1.1f);
        Singleton.Instance.Atack = false;
    }
}
