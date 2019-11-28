using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeatBoss : Boss
{
    private bool playerInRoom;
    private bool introStarted;
    private bool phasesStarted;
    private bool secondPhaseStarted;
    private bool bossIsDead;
    private float rotateForce;
    private Camera playerCamera;
    private Rigidbody meatBody;
    private Collider meatCollider;
    private Vector3 lastFrameVelocity;
    private float meatSpeed;
    private Transform player;
    private float attackSpeed;
    private bool canAttack;
    private bool isBouncing;

    private HUDController gameHUD;
    private AudioSource gameAudio;

    private Animator meatBossAnim;

    public GameObject meatTracePrefab;

    public bool PlayerInRoom {
      get {return playerInRoom;}
      set {playerInRoom = value;}
    }

    void Awake()
    {
      playerCamera = GameObject.Find("Camera").GetComponent<Camera>();
      meatBody = GetComponent<Rigidbody>();
      player = GameObject.Find("Player").transform;
      meatBossAnim = GetComponent<Animator>();
      meatCollider = GetComponent<Collider>();
    }
    // Start is called before the first frame update
    void Start()
    {
      isBouncing = false;
      canAttack = true;
      attackSpeed = 7500f;
      rotateForce = 40f;
      bossPhase = 0;
      bossLife = 2500f;
      Singleton.Instance.CurrentBossLife = bossLife;
      maxBossLife = bossLife;
      secondPhasePercentage = 75f;
      thirdPhasePercentage = 20f;
      secondPhaseLife = (secondPhasePercentage * bossLife) / 100f;
      thirdPhaseLife = (thirdPhasePercentage * bossLife) / 100f;
      playerInRoom = false;
      introStarted = false;

      bossIsDead = false;
      phasesStarted = false;
      secondPhaseStarted = false;

      gameHUD = GameObject.Find("GameController").GetComponent<HUDController>();
      gameAudio = GameObject.Find("GameController").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      BossPhaseState();
      MeatBossStateMachine();
      if(bossBar!=null)
      {
        bossBar.fillAmount = bossLife / maxBossLife;
      }
    }

    void FixedUpdate()
    {
      if(bossPhase != 0 && bossPhase !=4 && isBouncing) {
        Rotate();
        lastFrameVelocity = meatBody.velocity;
        lastFrameVelocity.y = 0;
      }
    }

    void MeatBossStateMachine()
    {
      switch(bossPhase)
      {
        case 0:
          if(playerInRoom) {
              MeatBossIntroduction();
              playerCamera.gameObject.GetComponent<CameraFollow>().CameraZoomOut();
              if(!introStarted) {
                gameAudio.clip = bossMusic;
                gameAudio.Play();
                meatBossAnim.enabled = true;
                StartCoroutine(StartPhaseOne(3f));
                introStarted = true;
              }
          }
        break;

        case 1:
          Debug.Log("FIRST PHASE");
          playerCamera.gameObject.GetComponent<CameraFollow>().CameraZoomIn();
          MeatBounceBehaviour();
        break;

        case 2:
          Debug.Log("SECOND PHASE");
          if(!secondPhaseStarted) {
            /*
            pcAudio.clip = angryBossClip;
            pcAudio.Play();
            */
            InvokeRepeating("SpawnTrace", 0.1f, 0.15f);
            secondPhaseStarted = true;
          }
          MeatBounceBehaviour();
        break;

        case 3:
          Debug.Log("THIRD PHASE");
          MeatBounceBehaviour();
          //MeatBossIntroduction();
        break;

        case 4:
          Debug.Log("FOURTH PHASE");
          if(!bossIsDead) {
            MeatBossDies();
          }
        break;

        default:
        break;
      }
    }

    void MeatBossIntroduction()
    {
      Debug.Log("MeatBoss Intro");
    }

    IEnumerator StartPhaseOne(float _timer)
    {
      yield return new WaitForSeconds(_timer);
      bossPhase = 1;
      phasesStarted = true;

      gameHUD.ShowBossLife();
      bossBar = GameObject.Find("BossBar").transform.GetChild(1).gameObject.GetComponent<Image>();
      gameHUD.ChangeBossBarText("Meat Boss");
    }

    void MeatBounceBehaviour()
    {
      Debug.Log("Meat Bounce Behaviour");
      if(!isBouncing) {
        Debug.Log("Meat Boss Look At Player");
        transform.LookAt(player);
        Quaternion meatRotation = transform.rotation;
        meatRotation.x = 0f;
        meatRotation.z = 0f;
        transform.rotation = meatRotation;
        if(canAttack) {
          canAttack = false;
          StartCoroutine(MeatBossAttack(2f));
        }
      }
    }

    IEnumerator MeatBossAttack(float _timer)
    {
      Debug.Log("Meat Boss ATTACK!");
      yield return new WaitForSeconds(_timer);
      meatBody.AddForce(transform.forward * attackSpeed, ForceMode.Impulse);
      isBouncing = true;
      StartCoroutine(StopBouncing(4f));
    }

    void SpawnTrace()
    {
      Vector3 spawnPos = transform.position;
      spawnPos.y = 0.01f;
      float randomYRotation = Random.Range(0f, 360f);
      Instantiate(meatTracePrefab, spawnPos, Quaternion.Euler(new Vector3(90, randomYRotation, 0)));
    }

    void MeatBossDies()
    {
      Debug.Log("Meat Boss IS DEAD!!");
      CancelInvoke();
      StopAllCoroutines();
      bossIsDead = true;
      StartCoroutine(StopBouncing(0f));
      //Instanciar partículas aquí
      transform.GetChild(0).gameObject.SetActive(false);
      meatCollider.enabled = false;
    }

    void Rotate()
    {
      transform.Rotate(Vector3.up * rotateForce *Time.deltaTime);
    }

    void Bounce(Vector3 collisionNormal)
    {
        isBouncing = true;
        collisionNormal.y = 0;
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal.normalized);
        //var randomDirection = new Vector3(Random.Range(direction.x, direction.x + 2.5f), 0f, Random.Range(direction.z, direction.z + 2.5f));
        //direction += randomDirection;
        direction.y = 0;
        /*
        direction.x += Random.Range(-2f, 2f);
        direction.z += Random.Range(-2f, 2f);
        */
        meatBody.velocity = direction * speed;
    }

    IEnumerator StopBouncing(float _timer)
    {
      yield return new WaitForSeconds(_timer);
      Debug.Log("STOP Bouncing!");
      meatBody.velocity = Vector3.zero;
      isBouncing = false;
      canAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag(MyTags.BULLET_TAG) && bossPhase != 0) {
          Debug.Log("Bounce");
          Bounce(collision.contacts[0].normal);
        } else if(collision.gameObject.CompareTag(MyTags.BULLET_TAG)) {
          bossLife -= collision.gameObject.GetComponent<BulletBehavior>().bullet.damage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(MyTags.PLAYER_TAG))
        {
            if(other.GetComponentInParent<IDamageable>() != null)
            {
                Vector3 direction = other.transform.position - transform.position;
                other.GetComponentInParent<IDamageable>().Hurt(150);
                other.GetComponentInParent<IDamageable>().EnableKnockback(direction, 5, 60);
            }
        }
    }

}//class





















//space
