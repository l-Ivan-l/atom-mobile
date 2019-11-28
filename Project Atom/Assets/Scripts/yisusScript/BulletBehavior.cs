using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletBehavior : MonoBehaviour
{
    public Bullet bullet;
    Joystick joy;
    public Transform shootPoint;
    float lifeTime;
    Rigidbody rigid;
    private float recoil;
    public LayerMask Enemies;
    AudioSource audioSource;

    [Header("Bounce")]
    private Vector3 lastFrameVelocity;
    private float lifeBullet;

    public List<Bullet> actualBullet;
    GameController pullController;

    int probability;
    private void Awake()
    {
        
        pullController = GameObject.Find("GameController").GetComponent<GameController>();
        audioSource = GameObject.Find("GameController").GetComponent<AudioSource>();
        rigid = gameObject.GetComponent<Rigidbody>();
        joy = GameObject.Find("JoyShoot").GetComponent<Joystick>();
        lifeTime = bullet.lifeTime;
        SeleccionarBala();
        BulletAspect();
        //shootPoint = GameObject.Find("ShootPoint").transform;

        //GravityBullet();
        //SeleccionarBala();
        //BulletAspect();
        //EleguirDireccion();

    }


    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime<= 0)
        {
            DisableBullet();
        }
    }
    private void FixedUpdate()
    {
        lastFrameVelocity = rigid.velocity;
        FollowTarget();
        GravityBullet();

    }

    private void OnEnable()
    {
        //GravityBullet();
        
        StartCoroutine(EleguirDireccion());
        Debug.Log("Disparando");
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletBounce(collision.contacts[0].normal);
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<IDamageable>().Hurt(bullet.damage);
            collision.gameObject.GetComponent<IDamageable>().EnableKnockback(direction, 5f, bullet.knockback);
        }
        if (collision.gameObject.GetComponent<IAlteredEffects>() != null)
        {
            if (BulletPoisoned())
                collision.gameObject.GetComponent<IAlteredEffects>().Poisoned(bullet.damage * 0.10f, 5);
        }
        /*Knockback(collision);*/
        if(!bullet.bounce) DisableBullet();
    }

    IEnumerator EleguirDireccion()
    {
        probability = Random.Range(0, 101);
        if(BulletPoisoned())
        {
            gameObject.GetComponent<MeshRenderer>().material = bullet.poisonMaterial;
        }
        Recoil();
        //particle.Play();
        lifeTime = bullet.lifeTime;
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.rotation = shootPoint.rotation;
        gameObject.transform.position = shootPoint.position;
        rigid.AddForce((shootPoint.forward * bullet.speed) + (shootPoint.right * recoil * bullet.speed) + (shootPoint.up * bullet.upForce), ForceMode.Impulse);
        audioSource.PlayOneShot(bullet.sound,0.5f);
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<TrailRenderer>().enabled = true;
    }

    public void BulletAspect()
    {
        gameObject.GetComponent<MeshRenderer>().material = bullet.material;
        gameObject.GetComponent<MeshFilter>().mesh = bullet.mesh;
        gameObject.GetComponent<TrailRenderer>().material = bullet.trailMaterial;
        gameObject.GetComponent<TrailRenderer>().widthCurve = bullet.trail.widthCurve;
        gameObject.GetComponent<TrailRenderer>().time = bullet.trail.time;
        gameObject.GetComponent<TrailRenderer>().colorGradient = bullet.trail.colorGradient;
        gameObject.GetComponent<TrailRenderer>().textureMode = bullet.trail.textureMode;
        gameObject.GetComponent<SphereCollider>().radius = bullet.hitRadio;
        //gameObject.GetComponent<TrailRenderer>().enabled = bullet.trail.enabled;


    }

    public void SeleccionarBala()
    {
        if (bullet.enemyBullet) bullet = actualBullet[0];
        else bullet = actualBullet[Singleton.Instance.ActualBullet];
    }

    //void Knockback(Collision enemy)
    //{
    //    if(enemy.gameObject.CompareTag("enemy1") && enemy.gameObject.GetComponent<PathFinding>())
    //    {
    //        PathFinding path = enemy.gameObject.GetComponent<PathFinding>();
    //        Vector3 direction = enemy.transform.position - transform.position;

    //        //path.directionImpulse = direction;
    //        path.knockbackForce = bullet.knockback;
    //        path.timeKnockback = 0.1f;
    //    }
    //}

    void Recoil()
    {
        recoil = Random.Range(-bullet.recoil, bullet.recoil);
    }

    void FollowTarget()
    {
        if(bullet.follow)
        {
            Collider[] enemiesToFollow = Physics.OverlapSphere(transform.position, bullet.rangeFollow, Enemies);

            if(enemiesToFollow.Length >0)
            {
                rigid.velocity = new Vector3((enemiesToFollow[0].transform.position.x - transform.position.x) * bullet.speed / 2, 0, (enemiesToFollow[0].transform.position.z - transform.position.z) * bullet.speed / 2);
                if (enemiesToFollow.Length <= 0)
                {
                    rigid.velocity = Vector3.zero;
                }
            }



        }
    }

    void DisableBullet()
    {
        ActiveImpactEffect();
        
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.position = shootPoint.position;
        gameObject.GetComponent<MeshRenderer>().material = bullet.material;
        rigid.velocity = Vector3.zero;
        lifeTime = bullet.lifeTime;
        gameObject.SetActive(false);
    }

    public void GravityBullet()
    {
        if(bullet.useGravity)
        {
            rigid.velocity += Vector3.up * Physics.gravity.y * bullet.gravityForce;
        }

    }

    void BulletBounce(Vector3 collisionNormal)
    {
        if(bullet.bounce)
        {
            collisionNormal.y = 0;
            var speed = lastFrameVelocity.magnitude;
            var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal.normalized);
            direction.y = 0;

            transform.LookAt(direction);
            transform.rotation = transform.localRotation;
            rigid.velocity = direction * bullet.speed;
        }

    }
    void ActiveImpactEffect()
    {
        pullController.pullParticles[pullController.iPullParticles].transform.position = transform.position;
        pullController.pullParticles[pullController.iPullParticles].GetComponent<ParticleSystem>().Play();
        pullController.iPullParticles++;
        if (pullController.iPullParticles >= pullController.numberOfParticles)
            pullController.iPullParticles = 0;
    }

    //-----------------AlteredEffects------------------------------------
    bool BulletPoisoned()
    {
        if(bullet.bulletPoisoned)
        {
            
            if (probability <= bullet.poisonPorcentage)
            {
                return true;
            }
                
        }
        return false;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, bullet.rangeFollow);
    }
}
