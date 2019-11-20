using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class EnemyFlyBehavior : Enemy,IDamageable,IAlteredEffects
{
    [Header("Movement")]
    private Vector3 initialVelocity;

    [Header("Rotate")]
    [SerializeField] private float rotateForce;
    private Vector3 lastFrameVelocity;

    [Header("Atacar")]
    [SerializeField] private ParticleSystem explotion;

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SphereCollider sphereCol;
    [SerializeField] private CapsuleCollider capsuleCol;


    // Start is called before the first frame update
    private void Start()
    {
        dropController = GameObject.Find("GameController").GetComponent<DropItemController>();
        initialVelocity = new Vector3(speed, 0, speed);
        rigid.velocity = initialVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Die();
        Rotate();
        lastFrameVelocity = rigid.velocity;
        lastFrameVelocity.y = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Atack();
            if(other.GetComponentInParent<IDamageable>() != null)
            {
                Vector3 direction = other.transform.position - transform.position;
                other.GetComponentInParent<IDamageable>().Hurt(150);
                other.GetComponentInParent<IDamageable>().EnableKnockback(direction, 5, 60);
                Debug.Log("Explotar");
            }
        }
    }
    public override void Move()
    {
        throw new System.NotImplementedException();
        //Implementar el movimiento inicial 
    }
    void Bounce(Vector3 collisionNormal)
    {
        collisionNormal.y = 0;
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal.normalized);
        direction.y = 0;

        rigid.velocity = direction * this.speed;
    }

    public override void Atack()
    {
        rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f);
        explotion.gameObject.SetActive(true);
        explotion.Play();
        StartCoroutine(MorirCo());
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotateForce *Time.deltaTime);

    }

    public override void Die()
    {
        if (life <= 0)
        {

            StartCoroutine(MorirCo());

        }
    }

    IEnumerator MorirCo()
    {
        
        gameObject.layer = 1;
        rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f);
        rigid.constraints = RigidbodyConstraints.None;
        rigid.useGravity = true;
        particle.Stop();
        capsuleCol.enabled = false;
        sphereCol.enabled = true;
        rotateForce = 0f;
        yield return new WaitForSeconds(1f);
        DropItem("LifeOrb");
        gameObject.SetActive(false);
        this.gameObject.transform.parent.gameObject.GetComponent<EnemiesGenerator>().EnemiesInRoom.Remove(this.gameObject);
    }

    void IDamageable.EnableKnockback(Vector3 direction, float time, float force)
    {
        //yield return new WaitForSeconds(0f);
    }

    void IDamageable.Hurt(float Damage)
    {
        hurtEffect.Play();
        life -= Damage;
    }
    // AlteredEffects 

    void IAlteredEffects.Poisoned(float damage, int times)
    {
        StartCoroutine(CoPoisoned(damage, times));
    }

    //---------------Coroutines AlteredEffects-------------------
    IEnumerator CoPoisoned(float damage, int times)
    {
        int iterator = 0;

        while (iterator < times)
        {
            life -= damage;
            enemyShape.material = poisonedMaterial;
            Debug.Log("POISONED!!!");
            yield return new WaitForSeconds(0.4f);
            enemyShape.material = standarMaterial;
            yield return new WaitForSeconds(0.2f);
            iterator++;
        }
        enemyShape.material = standarMaterial;
    }
}
