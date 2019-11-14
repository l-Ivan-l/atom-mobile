using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCBoss : Boss
{
    public GameObject[] Spikes;
    public Light[] AlarmLights;
    public GameObject armHole;
    //public GameObject armDirection;
    public GameObject Arm;
    public Animator[] spikesAnim;

    private AudioSource pcAudio;
    public AudioClip angryBossClip;

    private Animator armAnimator;
    private AnimatorClipInfo[] armCurrentClipInfo;
    private float armsAngle;
    private int nArms;
    private float minFollowTime;
    private float maxFollowTime;
    private float stopTime;
    private float minLightIntensity;
    private float maxLightIntensity;
    private bool playerInRoom;
    private float t;
    private bool introStarted;
    private Transform player;
    private Camera playerCamera;
    private bool shadowIsActive;
    private bool shadowFollow;
    private bool phasesStarted;
    private bool spikesActivated;
    private bool secondPhaseStarted;

    private bool bossIsDead;

    private HUDController gameHUD;
    private AudioSource gameAudio;

    public bool PlayerInRoom {
      get {return playerInRoom;}
      set {playerInRoom = value;}
    }

    void Awake()
    {
      bossAnim = GetComponent<Animator>();
      armAnimator = Arm.GetComponent<Animator>();
      player = GameObject.Find("Player").transform;
      //playerCamera = Camera.main;
      playerCamera = GameObject.Find("Camera").GetComponent<Camera>();
      pcAudio = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
      bossPhase = 0;
      bossLife = 2000f;
      Singleton.Instance.CurrentBossLife = bossLife;
      maxBossLife = bossLife;
      secondPhasePercentage = 75f;
      thirdPhasePercentage = 25f;
      secondPhaseLife = (secondPhasePercentage * bossLife) / 100f;
      thirdPhaseLife = (thirdPhasePercentage * bossLife) / 100f;
      minLightIntensity = 5f;
      maxLightIntensity = 100f;
      playerInRoom = false;
      t = 0.0f;
      introStarted = false;
      shadowIsActive = false;
      shadowFollow = true;

      minFollowTime = 1.5f;
      maxFollowTime = 2f;
      stopTime = 0.75f;

      bossIsDead = false;
      phasesStarted = false;
      spikesActivated = false;
      secondPhaseStarted = false;

      gameAudio = GameObject.Find("GameController").GetComponent<AudioSource>();
      gameHUD = GameObject.Find("GameController").GetComponent<HUDController>();
    }

    // Update is called once per frame
    void Update()
    {
      BossPhaseState();
      PCStateMachine();
      if(phasesStarted) {
        SpikesBehaviour();
        phasesStarted = false;
      }
      if(bossBar!=null)
      {
        bossBar.fillAmount = bossLife / maxBossLife;
      }
    }

    void PCStateMachine()
    {
      switch(bossPhase)
      {
        case 0:
          if(playerInRoom) {
              PCIntroduction();
              playerCamera.gameObject.GetComponent<CameraFollow>().CameraZoomOut();
              if(!introStarted) {
                gameAudio.clip = bossMusic;
                gameAudio.Play();
                StartCoroutine(StartPhaseOne(5f));
                introStarted = true;
              }
          }
        break;

        case 1:
          playerCamera.gameObject.GetComponent<CameraFollow>().CameraZoomIn();
          ShadowBehaviour();
        break;

        case 2:
          if(!secondPhaseStarted) {
            pcAudio.clip = angryBossClip;
            pcAudio.Play();
            secondPhaseStarted = true;
          }
          minFollowTime = 0.75f;
          maxFollowTime = 1.5f;
          stopTime = 0.7f;
          armAnimator.speed = 1.5f;
          ShadowBehaviour();
        break;

        case 3:
          minFollowTime = 0.75f;
          maxFollowTime = 1f;
          stopTime = 0.5f;
          armAnimator.speed = 1.75f;
          ShadowBehaviour();
          PCIntroduction();
        break;

        case 4:
          if(!bossIsDead) {
            PCDies();
          }
        break;

        default:
        break;
      }
    }

    void PCIntroduction()
    {
      if(!spikesActivated) {
        for(int i = 0; i < spikesAnim.Length; i++) {
          spikesAnim[i].SetTrigger("PlayerInRoom");
        }
        spikesActivated = true;
      }
      for(int i = 0; i < AlarmLights.Length; i++) {
        AlarmLights[i].intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);
      }
      t += 2f * Time.deltaTime;
      if (t > 1.0f)
      {
          float temp = maxLightIntensity;
          maxLightIntensity = minLightIntensity;
          minLightIntensity = temp;
          t = 0.0f;
      }
    }

    void SpikesBehaviour()
    {
      int attackType = 0;
      float timer = 0f;
      if(bossPhase == 1) {
        attackType = 3;
        timer = Random.Range(2.5f, 4f);
      } else if(bossPhase == 2) {
        attackType = Random.Range(1, 3);
        timer = Random.Range(2f, 3f);
      } else if(bossPhase == 3) {
        attackType = Random.Range(1, 3);
        timer = Random.Range(2f, 2.5f);
      }

      StartCoroutine(SpikesAttack(attackType, timer));
    }

    IEnumerator SpikesAttack(int _attackType, float _timer)
    {
      yield return new WaitForSeconds(_timer);
      switch(_attackType)
      {
        case 1:
          for(int i = 0; i < ((spikesAnim.Length - 4) / 2); i++) {
            spikesAnim[i].SetTrigger("SpikeAttack");
            spikesAnim[i].SetTrigger("SpikeHide");
          }
        break;

        case 2:
          for(int i = ((spikesAnim.Length - 4) / 2); i < spikesAnim.Length - 4; i++) {
            spikesAnim[i].SetTrigger("SpikeAttack");
            spikesAnim[i].SetTrigger("SpikeHide");
          }
        break;

        case 3:
          int randomSpike = 0;
          for(int i = 0; i < 3; i++) {
            randomSpike = Random.Range(0, spikesAnim.Length);
            spikesAnim[randomSpike].SetTrigger("SpikeAttack");
            spikesAnim[randomSpike].SetTrigger("SpikeHide");
          }
        break;

        default:
        break;
      }
      SpikesBehaviour();
    }

    IEnumerator StartPhaseOne(float _timer)
    {
      yield return new WaitForSeconds(_timer);
      for(int i = 0; i < spikesAnim.Length - 4; i++) {
        spikesAnim[i].SetTrigger("StartPhases");
      }
      bossPhase = 1;
      phasesStarted = true;
      gameHUD.ShowBossLife();
      bossBar = GameObject.Find("BossBar").transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    void ShadowBehaviour()
    {
      if(!shadowIsActive) {
        ShadowAppear();
        StartCoroutine(ShadowCanFollow(Random.Range(minFollowTime, maxFollowTime)));
      } else {
        ShadowFollowPlayer();
      }
    }

    void ShadowAppear()
    {
      armHole.SetActive(true);
      armHole.transform.position = new Vector3((Mathf.MoveTowards(armHole.transform.position.x, player.position.x, Time.deltaTime * 8f)), armHole.transform.position.y,
                                              (Mathf.MoveTowards(armHole.transform.position.z, player.position.z, Time.deltaTime * 8f)));
      shadowIsActive = true;
    }

    void ShadowFollowPlayer()
    {
      if(shadowFollow) {
        Vector3 armPos = armHole.transform.position;
        armPos.x = player.position.x;
        armPos.z = player.position.z;
        armHole.transform.position = armPos;
      }
    }

    IEnumerator ShadowCanFollow(float _timer)
    {
      yield return new WaitForSeconds(_timer);
      shadowFollow = false;
      yield return new WaitForSeconds(stopTime);
      ArmAttack();
      yield return new WaitForSeconds(armCurrentClipInfo[0].clip.length);
      shadowFollow = true;
      StartCoroutine(ShadowCanFollow(Random.Range(minFollowTime, maxFollowTime)));
    }

    void ArmAttack()
    {
      Vector3 armPos = Arm.transform.position;
      armPos.x = armHole.transform.position.x;
      armPos.z = armHole.transform.position.z;
      Arm.transform.position = armPos;

      armAnimator.SetTrigger("Attack");
      armCurrentClipInfo = armAnimator.GetCurrentAnimatorClipInfo(0);
    }

    void PCDies()
    {
      StopAllCoroutines();
      armHole.SetActive(false);
      bossIsDead = true;
    }

    void OnCollisionEnter(Collision target)
    {
      if(target.gameObject.CompareTag(MyTags.BULLET_TAG)) {
        bossLife -= target.gameObject.GetComponent<BulletBehavior>().bullet.damage;
      }
    }

}//class


























//space
