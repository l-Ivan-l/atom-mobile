using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitBoxEnemy : MonoBehaviour
{
    public GameObject enemy;
    public NavMeshAgent enemyBehabior;
   // public PathFinding path;
    private Rigidbody rigid;
    private Bullet bullet;
    private Animator robotAnim;
   

    void Awake()
    {
      robotAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        
        rigid = enemy.GetComponent<Rigidbody>();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("bullet") && !path.die)
        //    {
        //    robotAnim.Play("RobotHit");
        //    bullet = collision.gameObject.GetComponent<BulletBehavior>().bullet;
        //    path.life -= bullet.damage;
        //}
    }
    
}
