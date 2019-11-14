using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyShootBehavior : Enemy, IDamageable
{
    private Transform target;
    [SerializeField]
    private Vector3 dest;
    NavMeshAgent agent;
    public float distance;

    public float stopDistance;
    public float retreadDistance;

    public float startTimeBtwShoot;
    public float timeBtwShoot;

    public Transform firePoint;
    private int ibullet = 0;
    public GameObject bullet;
    private List<GameObject> Bullets;

    public Vector3 directionImpulse;

    private Bullet _bullet;

    void Start()
    {
        dropController = GameObject.Find("GameController").GetComponent<DropItemController>();
        Bullets = new List<GameObject>();
        CrearPullBalas();
        target = GameObject.Find("Player").transform;
        gameObject.transform.LookAt(target);
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        dest = agent.destination;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (life <= 0)
        {
            Die();
        }
        else Move();
    }

    public override void Move()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > stopDistance)
        {
            gameObject.transform.LookAt(target);
            anim.Play("Walk2");
            dest = target.position;
            agent.destination = dest;
        }
        else if (dist < stopDistance && dist > retreadDistance)
        {
            //anim.Play("Idle");
            gameObject.transform.LookAt(target);
            agent.destination = this.transform.position;
            Atack();
        }
        else if (dist < retreadDistance)
        {
            anim.Play("Walk2");
            dest = target.position;
            agent.destination = -dest;
        }

    }

    public override void Atack()
    {

        if (timeBtwShoot <= 0)
        {
            if (Bullets.Count > 0)
            {
                Bullets[ibullet].SetActive(true);
                anim.Play("Shooting");
                ibullet++;
                if (ibullet >= 2) ibullet = 0;
            }
            timeBtwShoot = startTimeBtwShoot;
        }
        else
        {
            timeBtwShoot -= Time.deltaTime;
        }
    }

    void CrearPullBalas()
    {
        GameObject _Bullet;
        for (int i = 0; i < 3; i++, ibullet++)
        {
            _Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            _Bullet.name = "bullet" + ibullet.ToString();
            Bullets.Add(_Bullet);
            //Debug.Log(Bullets[ibullet]);
        }
        ibullet = 0;
    }





    void IDamageable.EnableKnockback(Vector3 direction, float time, float force)
    {
        StartCoroutine(CoKnockback(direction, time, force));
    }
    IEnumerator CoKnockback(Vector3 direction, float time, float force)
    {

        for (int i = 0; i < time; i++)
        {
            rigid.AddForce(direction.normalized * force, ForceMode.Impulse);
            yield return new WaitForSeconds(0.01f);
            Debug.Log("EMPUJANDO!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
    public override void Die()
    {
        StartCoroutine(Morir());
    }
    IEnumerator Morir()
    {    
        gameObject.tag = "Untagged";
        agent.speed = 0;
        die = true;
        anim.Play("Death");
        yield return new WaitForSeconds(2f);
        DropItem("LifeOrb");
        gameObject.SetActive(false);

        this.gameObject.transform.parent.gameObject.GetComponent<EnemiesGenerator>().EnemiesInRoom.Remove(this.gameObject);
    }

    void IDamageable.Hurt(float Damage)
    {
        hurtEffect.Play();
        life -= Damage;
        if(!die)anim.Play("Damage");
    }
}
