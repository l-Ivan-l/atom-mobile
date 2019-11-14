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
        else if(other.CompareTag("Ray"))
        {
            Singleton.Instance.ActualWeapon = 0;
            Singleton.Instance.ActualBullet = 0;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if(other.CompareTag("Rev"))
        {
            Singleton.Instance.ActualWeapon = 1;
            Singleton.Instance.ActualBullet = 1;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Ra"))
        {
            Singleton.Instance.ActualWeapon = 2;
            Singleton.Instance.ActualBullet = 2;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Res"))
        {
            Singleton.Instance.ActualWeapon = 3;
            Singleton.Instance.ActualBullet = 3;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Tommy"))
        {
            Singleton.Instance.ActualWeapon = 4;
            Singleton.Instance.ActualBullet = 4;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Frank"))
        {
            Singleton.Instance.ActualWeapon = 5;
            Singleton.Instance.ActualBullet = 5;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Blas"))
        {
            Singleton.Instance.ActualWeapon = 6;
            Singleton.Instance.ActualBullet = 6;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Desert"))
        {
            Singleton.Instance.ActualWeapon = 7;
            Singleton.Instance.ActualBullet = 7;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("RevNorm"))
        {
            Singleton.Instance.ActualWeapon = 8;
            Singleton.Instance.ActualBullet = 8;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Glock"))
        {
            Singleton.Instance.ActualWeapon = 9;
            Singleton.Instance.ActualBullet = 9;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Gatling"))
        {
            Singleton.Instance.ActualWeapon = 10;
            Singleton.Instance.ActualBullet = 10;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("M1"))
        {
            Singleton.Instance.ActualWeapon = 11;
            Singleton.Instance.ActualBullet = 11;
            StartCoroutine(ChangeWepon());
            other.gameObject.SetActive(false);
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
