using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitBoxPlayer : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement pMov;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = player.GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ICollectable>() != null)
        {
            other.GetComponent<ICollectable>().Collect();
        }

        if(other.CompareTag("WinOrb"))
        {
            SceneManager.LoadScene("final");
        }




        else if (other.CompareTag(MyTags.SPIKE_TAG))
        {
          var heading = other.transform.position - player.transform.position;
          var distance = heading.magnitude;
          var direction = heading / distance;
          Singleton.Instance.PlayerLife -= 100f;
         // pMov.direction = -direction;
          //pMov.GetComponent<IDamageable>().EnableKnockback(direction, 0.15f, 30);
          //pMov.timeKnockback = 0.15f;
          //StartCoroutine(pMov.ActivarHitBox(1f));
        }
        else if (other.CompareTag(MyTags.ARM_BOSS_TAG))
        {
          var heading = other.transform.position - player.transform.position;
          var distance = heading.magnitude;
          var direction = heading / distance;
          Singleton.Instance.PlayerLife -= 85f;
          //pMov.direction = -direction;
         // pMov.timeKnockback = 0f;
        }
    }

    IEnumerator ChangeWepon()
    {
        Singleton.Instance.ChangeWepon = true;
        yield return new WaitForSeconds(0.1f);
        Singleton.Instance.ChangeWepon = false;
    }
    //IEnumerator Empujar(Vector3 _directiom)
    //{
    //    rigid.AddForce(-_directiom * 50, ForceMode.Impulse);
    //    gameObject.GetComponent<BoxCollider>().enabled = false;
    //    yield return new WaitForSeconds(0.5f);
    //    pMov.enabled = true;
    //    yield return new WaitForSeconds(0.5f);
    //    gameObject.GetComponent<BoxCollider>().enabled = true;
    //}

}
