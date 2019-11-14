using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaPinchos : MonoBehaviour
{

    public float damage;
    public float speed;
    public float timeBtwApear;
    float timeApear;
    public Transform begin, middle, final, picos;
    public Collider coll;

    private void Start()
    {
        timeApear = timeBtwApear;
        
    }

    private void Update()
    {
        timeApear -= Time.deltaTime;
        if(timeApear <= 0)
        {
            timeApear = timeBtwApear;
            StartCoroutine(SacarPinchos());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entrando a trampa");
            if (other.GetComponentInParent<IDamageable>() != null)
            {
                other.GetComponentInParent<IDamageable>().Hurt(damage);
            }
        }
            
    }



    IEnumerator SacarPinchos()
    {
        picos.position = middle.position;
        yield return new WaitForSeconds(0.5f);
        picos.position = final.position;
        coll.enabled = true;
        timeApear = timeBtwApear;
        yield return new WaitForSeconds(1.5f);
        picos.position = begin.position;
        coll.enabled = false;
        timeApear = timeBtwApear;

    }
}
