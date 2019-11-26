using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDamageable, IDash
{
    //Pasar las variables de los joysticks a un controller/singleton
    [Header("Movimiento")]
    public Joystick Joystick;
    public float speed;
    float vertical;
    float horizontal;
    Vector3 movement;
    Rigidbody rigid;
    //Cambiar todas las variables de disparo a su propio script cuando tenga más armas.
    public Joystick JoystickShoot;
    bool isShooting;
    [Header("Dash")]
    public GameObject hitBox;
    public float dashForce;
    public float dashDuration;
    public int repeatTimes;
    [SerializeField]TrailRenderer dashEffect;
    bool dashEnable;

    [Header("Vida")]
    public float vida;
    //public Image healthBar;

    [Header("Collect Item")]
    public LayerMask WhatIsItem;
    public float collectRatio;

    private Animator animPlayer;
    private bool morir;
    private Transform bossTransform;
    private Transform treasureTransform;

    //bool activeMove = true;
    void Start()
    {
        //Singleton.Instance.PlayerLife = vida;
        dashEffect.enabled = false;
        morir = false;
        animPlayer = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody>();
        dashEnable = true;
        Singleton.Instance.StopTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!morir && !Singleton.Instance.Atack && Singleton.Instance.CanMove)
        {
            WalkPlayer();
            RotatePlayer();
            CollectItem();
        }

    }

    private void Update()
    {
        StopPlayer();
        Animaciones();
        if (Singleton.Instance.PlayerLife <= 0)
        {
            StartCoroutine(Morir());
        }   
        if (Singleton.Instance.PlayerLife > vida) Singleton.Instance.PlayerLife = vida;
        //healthBar.fillAmount = Singleton.Instance.PlayerLife / vida;
    }

    void RotatePlayer()
    {
        if (JoystickShoot.Horizontal != 0 || JoystickShoot.Vertical != 0)
        {
            var rotationDirection = new Vector3(JoystickShoot.Horizontal, 0, JoystickShoot.Vertical);
            transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            isShooting = true;
        }
        else if(Joystick.Horizontal != 0.0f || Joystick.Vertical != 0.0f)
        {
            var rotationDirection = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
            transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            isShooting = false;
        }
        else isShooting = false;

    }

    void WalkPlayer()
    {

        vertical = Joystick.Vertical * speed;
        horizontal = Joystick.Horizontal * speed;
        movement = new Vector3(horizontal , rigid.velocity.y, vertical);
        rigid.velocity = movement;

        gameObject.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * 1.2f ;
    }

    IEnumerator Morir()
    {
        rigid.velocity = Vector3.zero;
        morir = true;
        hitBox.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        //Singleton.Instance.ActualWeapon = 0;
        //Singleton.Instance.ActualBullet = 0;
        hitBox.SetActive(true);
        morir = false;

    }
    public IEnumerator CoDash(int times)
    {
        for (int i = 0; i < times; i++)
        {
            dashEffect.enabled = true;
            hitBox.SetActive(false);
            dashEnable = false;
            rigid.AddForce(gameObject.transform.forward * dashForce, ForceMode.Acceleration);
            yield return new WaitForSeconds(dashDuration);
        }
        StartCoroutine(ActivarHitBox(0.2f));      
        yield return new WaitForSeconds(0.5f);
        
        dashEnable = true;
    }
    public void Dash()
    {
        if(dashEnable)
        {
            StartCoroutine(CoDash(repeatTimes));
        }

    }



    public IEnumerator ActivarHitBox(float time)
    {
        yield return new WaitForSeconds(time);
        hitBox.SetActive(true);
        dashEffect.enabled = false;
    }
    void IDamageable.EnableKnockback(Vector3 direction, float time, float force)
    {
        StartCoroutine(CoKnockback(direction, time, force));
    }
    IEnumerator CoKnockback(Vector3 direction,float time, float force)
    {

        for (int i = 0; i < time; i++)
        {
            Singleton.Instance.StopTime = 0.1f;
            rigid.AddForce(direction.normalized * force, ForceMode.Impulse);
            yield return new WaitForSeconds(0.01f);
            
        }
    }
  
    void Animaciones()
    {
        if(Singleton.Instance.PlayerLife <= 0)
        {
            //Animacion muerte
            animPlayer.Play("Death");
        }
        else if (JoystickShoot.Horizontal != 0 && Singleton.Instance.CanMove || JoystickShoot.Vertical != 0 && Singleton.Instance.CanMove)
        {


            if (Singleton.Instance.WeaponType == 1)
            {
                if (Singleton.Instance.ActualWeapon == 2)
                {
                    //Ra animation
                    animPlayer.Play("Ra's");
                }
                else
                {
                    //shoot animation
                    animPlayer.Play("Shooting");
                }
            }


        }
        else if(Singleton.Instance.WeaponType == 2)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            animPlayer.Play("Melee_Attack10");
        }
        else if(Joystick.Horizontal != 0 && Singleton.Instance.CanMove || Joystick.Vertical != 0 && Singleton.Instance.CanMove)
        {
            //Animación de caminar
            animPlayer.Play("Walking");
        }
        else
        {
            animPlayer.Play("Idle");
        }
    }

    void StopPlayer()
    {
        Singleton.Instance.StopTime -= Time.deltaTime;

        if(Singleton.Instance.StopTime <= 0)
        {
            Singleton.Instance.CanMove = true;
        }
        else
        {
            Singleton.Instance.CanMove = false;
        }
    }

    void CollectItem()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, collectRatio, WhatIsItem);

        if(items.Length > 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].GetComponent<Rigidbody>().velocity = transform.position - items[i].transform.position;
            }
        }
    }
    public void BossHack()
    {
      bossTransform = GameObject.Find("BossPoint").transform;
      this.gameObject.transform.position = bossTransform.position;
    }

    public void TreasureHack()
    {
      treasureTransform = GameObject.Find("TreasurePoint").transform;
      this.gameObject.transform.position = treasureTransform.position;
    }

    public void Teletransport(Transform _portal)
    {
      Vector3 newPos = this.transform.position;
      newPos.x = _portal.position.x;
      newPos.z = _portal.position.z;
      this.transform.position = newPos;
    }

    //-----------------------------Interface IDamgaable

    void IDamageable.Hurt(float Damage)
    {
        Singleton.Instance.PlayerLife -= Damage;
        hitBox.SetActive(false);
        StartCoroutine(ActivarHitBox(1.5f));
    }
}
