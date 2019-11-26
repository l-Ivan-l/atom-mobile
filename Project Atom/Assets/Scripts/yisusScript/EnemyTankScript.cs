using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTankScript : Enemy
{
    private Transform target;
    [SerializeField]
    private Vector3 dest;
    NavMeshAgent agent;
    public float distance;
    public float stopDistance;
    public float followDistance;
    private bool atacando = false;
    private bool empujando = false;
    public float impulseForce;

    //Variables que necesito
    private int state = 0;
    public float maxRageTime;
    public float maxFollowTime;
    private float rageTime = 0;
    private float followTime = 0;
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
    void Update()
    {
        Move();
    }

    public override void Atack()
    {
        atacando = true;
        transform.LookAt(target);
        StartCoroutine(ImpulseAtak());
        
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
                anim.Play("RobotIdle");
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
                agent.acceleration = 5;
                agent.speed = speed * 4;
                transform.LookAt(target);
                anim.Play("TankRun");
                agent.destination = dest;
                followTime += Time.deltaTime;
                if(followTime >= maxFollowTime)
                {
                    stopDistance = 100;
                    followDistance = 100;
                    followTime = maxFollowTime / 2;
                    state = 0;
                }
                break;
        }
        //if (dist > stopDistance && !atacando)
        //{
        //    gameObject.transform.LookAt(target);
            
        //}
        //else
        //{
        //    Atack();
        //    anim.Play("RobotIdle");
        //    agent.destination = transform.position;
        //}
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ImpulseAtak()
    {
        if(!empujando)
        {
            empujando = true;
            for (int i = 0; i < 8; i++)
            {
                rigid.AddForce((target.transform.position - transform.position) * impulseForce, ForceMode.Acceleration);
                yield return new WaitForSeconds(0.01f);
            }
            atacando = false;
        }
        
    }
}
