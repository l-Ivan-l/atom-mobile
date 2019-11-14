using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRobotBehavior : Enemy,IDamageable
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 dest;
    NavMeshAgent agent;
    public float distance;
    public Transform hitPoint;
    public LayerMask WhatIsPlayer;

    private Vector3 directionImpulse;
    private float damage = 125f;
    private bool activeCollision;
    bool canMove = true;
    void Start()
    {
        dropController = GameObject.Find("GameController").GetComponent<DropItemController>();
        die = false;
        target = GameObject.Find("Player").transform;
        gameObject.transform.LookAt(target);
        agent = gameObject.GetComponent<NavMeshAgent>();
        dest = agent.destination;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target);
        if (life <= 0)
        {
            Die();
        }
        else
        {
            Move();
        }

    }

    public override void Move()
    {
        if (canMove)
        {
            agent.speed = speed;
            float dist = Vector3.Distance(target.position, transform.position);
            if (Vector3.Distance(dest, target.position) > 0.5f)
            {
                anim.Play("RobotWalk");
                dest = target.position;
                agent.destination = dest;
            }
            else if (dist < 0.5f)
            {
                agent.destination = transform.position;
                Atack();
            }
            agent.destination = dest;
        }
        else agent.destination = transform.position;
       
    }

    public override void Atack()
    {
        anim.Play("RobotAttack");
        StartCoroutine(ActiveAtac());
        Collider[] atakZone = Physics.OverlapSphere(hitPoint.position, 2, WhatIsPlayer);
        if (activeCollision && !die)
        {
            if (atakZone.Length > 0)
            {
                for (int i = 0; i < atakZone.Length; i++)
                {
                    if (atakZone[i].gameObject.GetComponentInParent<IDamageable>() != null)
                    {
                        Vector3 direction = atakZone[i].transform.position - transform.position;
                        direction.y = 0;
                        atakZone[i].gameObject.GetComponentInParent<IDamageable>().Hurt(damage);
                        atakZone[i].gameObject.GetComponentInParent<IDamageable>().EnableKnockback(direction, 5f, 30);
                    }
                }
                atakZone = null;
            }
            activeCollision = false;
        }

        //Cear una colision de ataque
    }
    IEnumerator ActiveAtac()
    {
        yield return new WaitForSeconds(0.5f);
        activeCollision = true;
    }



    void IDamageable.EnableKnockback(Vector3 direction, float time, float force)
    {
        StartCoroutine(CoKnockback(direction, time, force));
    }
    IEnumerator CoKnockback(Vector3 direction, float time, float force)
    {

        for (int i = 0; i < time; i++)
        {
            canMove = false;
            rigid.AddForce(direction.normalized * force, ForceMode.Impulse);
            yield return new WaitForSeconds(0.01f);
            Debug.Log("EMPUJANDO!!!!!!!!!!!!!!!!!!!!!!!!!!");

        }
        canMove = true;
    }

    public override void Die()
    {
        StartCoroutine(Morir());
    }
    IEnumerator Morir()
    {
        agent.destination = transform.position;
        gameObject.tag = "Untagged";   
        die = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        anim.Play("RobotDeath");
        yield return new WaitForSeconds(1.5f);
        DropItem("LifeOrb");
        gameObject.SetActive(false);
        this.gameObject.transform.parent.gameObject.GetComponent<EnemiesGenerator>().EnemiesInRoom.Remove(this.gameObject);
    }
    void IDamageable.Hurt(float Damage)
    {
        hurtEffect.Play();
        anim.Play("RobotHit");
        life -= Damage;
    }
}
