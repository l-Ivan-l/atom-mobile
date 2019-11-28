using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTankScript : Enemy,IDamageable
{
    public float damage;
    private Transform target;
    [SerializeField]
    private Vector3 dest;
    NavMeshAgent agent;
    public float distance;
    public float stopDistance;
    public float followDistance;
    private bool rageAtack = false;
    private bool empujando = false;
    public float impulseForce;


    //Variables que necesito
    public Material RageMaterial;
    private int state = 0;
    public float maxRageTime;
    public float maxFollowTime;
    private float rageTime = 0;
    private float followTime = 0;
    public LayerMask WhatIsPlayer;
    // Start is called before the first frame update
    void Start()
    {
        dropController = GameObject.Find("GameController").GetComponent<DropItemController>();
        target = GameObject.Find("Player").transform;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        dest = agent.destination;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(life <= 0)
        {
            Die();
        }
        else
        {
            Move();
            Atack();
        }
        
    }

    public override void Atack()
    {
        Collider[] atakZone = Physics.OverlapSphere(transform.position, 0.6f, WhatIsPlayer);
        if (atakZone.Length > 0)
        {
            for (int i = 0; i < atakZone.Length; i++)
            {
                if (atakZone[i].gameObject.GetComponentInParent<IDamageable>() != null)
                {
                    Vector3 direction = atakZone[i].transform.position - transform.position;
                    direction.y = 0;
                    atakZone[i].gameObject.GetComponentInParent<IDamageable>().Hurt(damage);
                    atakZone[i].gameObject.GetComponentInParent<IDamageable>().EnableKnockback(direction, 5f, 35);
                }
            }
            atakZone = null;
        }

    }

    public override void Move()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        dest = target.position;
        switch (state)
        {
            case 0:
                //Esperando
                followTime -= Time.deltaTime;
                anim.Play("IdleTankv2");
                agent.destination = transform.position;
                if (dist < followDistance && followTime <= 0)
                {
                    followTime = 0;
                    state = 1;
                }
                    
                break;
            case 1:
                //Siguiendo
                
                agent.acceleration = 2;
                agent.speed = speed;
                anim.Play("TankWalk");
                agent.destination = dest;
                rageTime += Time.deltaTime;

                if(rageTime >= maxRageTime)
                {
                    rageTime = 0;
                    state = 2;
                }

                if (dist > stopDistance)
                {
                    rageTime = 0;
                    state = 0;
                }
                    
                break;
            case 2:
                rageAtack = true;
                enemyShape.material = RageMaterial;
                agent.acceleration = 5;
                agent.speed = speed * 4;
                transform.LookAt(target);
                anim.Play("TankRun");
                agent.destination = dest;
                followTime += Time.deltaTime;
                if(followTime >= maxFollowTime)
                {
                    enemyShape.material = standarMaterial;
                    stopDistance = 100;
                    followDistance = 100;
                    followTime = maxFollowTime / 2;
                    rageAtack = false;
                    state = 0;
                }
                break;
        }
    }

    public override void Die()
    {
        StartCoroutine(CoDie());
    }

    IEnumerator CoDie()
    {
        agent.speed = 0;
        agent.destination = transform.position;
        die = true;
        anim.Play("TankDying");
        yield return new WaitForSeconds(4f);
        DropItem("LifeOrb");
        gameObject.SetActive(false);

        this.gameObject.transform.parent.gameObject.GetComponent<EnemiesGenerator>().EnemiesInRoom.Remove(this.gameObject);
    }

    void IDamageable.Hurt(float Damage)
    {
        if(!rageAtack && !die)
        {
            //anim.Play("TankHit");
            stopDistance = 100;
            followDistance = 100;
            hurtEffect.Play();
            life -= Damage;
        }
        
    }

    void IDamageable.EnableKnockback(Vector3 direction, float time, float force)
    {
        //No pasa nada No tiene KnockBack este vato
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position,0.6f);
    }
}
