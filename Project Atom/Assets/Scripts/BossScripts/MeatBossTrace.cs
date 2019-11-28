using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBossTrace : MonoBehaviour
{
  private SpriteRenderer meatTraceRender;
  private Collider traceCollider;
  private Color traceColor;
  private float t;
  private bool disappear;
  private float damage;

  void Awake()
  {
    meatTraceRender = GetComponent<SpriteRenderer>();
    traceCollider = GetComponent<Collider>();
  }

  void Start()
  {
    t = 0.0f;
    disappear = false;
    traceColor = meatTraceRender.color;
    damage = 2.5f;
    StartCoroutine(TraceExpired(0.5f));
  }

  void Update()
  {
    if(disappear) {
      DisappearTrace();
    }
  }

  IEnumerator TraceExpired(float _timer)
  {
    yield return new WaitForSeconds(_timer);
    disappear = true;
  }

  void DisappearTrace()
  {
    traceColor.a = Mathf.Lerp(1f, 0f, t);
    meatTraceRender.color = traceColor;
    if(t < 1f) {
      t += 0.35f * Time.deltaTime;
    }
    if(t >= 0.5f) {
      traceCollider.enabled = false;
    }
    if(t >= 1f) {
      gameObject.SetActive(false);
    }
  }

  private void OnTriggerStay(Collider other)
  {
      if (other.CompareTag("Player"))
      {
          Debug.Log("Entrando a meat trace");
          if (other.GetComponentInParent<IDamageable>() != null)
          {
              other.GetComponentInParent<IDamageable>().Hurt(damage);
          }
      }

  }

}//class
