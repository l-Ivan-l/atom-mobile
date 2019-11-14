using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour
{
    [Header("Disparo")]
   // private int numActualWeapon;
    private Gun ActualWeapon;
    public Joystick JoystickShoot;
    public Transform firePoint;
    public GameObject bullet;
    bool isShooting;
    float shootCounter;
    public int ibullet = 0;
    public float velRecarga;
    public  float cargaTiro;
    public int activeBullets;
    public List<GameObject> Bullets;
    AudioSource Source;

    

    //public GameObject barraBala;
    //public GameObject BarraCartucho;
    //public Transform posBarras;

    public List<Gun> Weapons;



    // Start is called before the first frame update
    void Start()
    {
        Source = GameObject.Find("GameController").GetComponent<AudioSource>();
        activeBullets = Weapons[Singleton.Instance.ActualWeapon].tamañoCartucho;
        Singleton.Instance.WeaponType = Singleton.Instance.ActualWeapon;
        //Singleton.Instance.ActualWeapon = numActualWeapon;
        SeleccionarArma();
        Bullets = new List<GameObject>();
        CrearPullBalas();
        shootCounter = 0;
        velRecarga = 0;
        cargaTiro = 0;
        WeaponAspect();
    }

    // Update is called once per frame
    void Update()
    {
        if(Singleton.Instance.CanMove)
        {
            SeleccionarArma();
            //BarraCargaCartucho();
            //BarraCargaPosition();
            ShootPlayer();
            WeaponAspect();
        }
        ChangeWepon();

        Debug.Log("ArmaActual: " + Singleton.Instance.ActualWeapon);
    }

    void CrearPullBalas()
    {
        GameObject _Bullet;
        for (int i = 0; i < 15; i++, ibullet++)
        {
            _Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            _Bullet.name = "bullet" + ibullet.ToString();
            Bullets.Add(_Bullet);
            //Debug.Log(Bullets[ibullet]);
        }
        ibullet = 0;
    }

    void ShootPlayer()
    {
        Recargar();
        CargaTiro();
        //Mover a otra función
        if (JoystickShoot.Vertical == 0 && JoystickShoot.Horizontal == 0)
        {
            shootCounter = 0.15f;
            isShooting = false;
        }
        else isShooting = true;
        if (isShooting)
        {
            shootCounter -= Time.deltaTime;
            if (shootCounter <= 0 && ibullet < ActualWeapon.tamañoCartucho)
            {
                shootCounter = ActualWeapon.fireRate;
                if (Bullets.Count > 0 && Bullets[ibullet] && cargaTiro >= ActualWeapon.tiempoCargaTiro)
                {
                    if (ibullet >= ActualWeapon.tamañoCartucho) ibullet = 0;
                    Bullets[ibullet].SetActive(true);
                    cargaTiro = 0;
                    ibullet++;
                    activeBullets--;
                }

            }
        }
    }

    void Recargar()
    {
        if(ibullet >= ActualWeapon.tamañoCartucho)
        {
          if(velRecarga <= 0 && ActualWeapon.rechargeSound != null)Source.PlayOneShot(ActualWeapon.rechargeSound,ActualWeapon.volume);
            if(velRecarga >= ActualWeapon.tiempoRecarga)
            {
                ibullet = 0;

                activeBullets = Weapons[Singleton.Instance.ActualWeapon].tamañoCartucho;
                velRecarga = 0;
            }
            else
            {
                velRecarga += Time.deltaTime;
            }
        }
    }

    void CargaTiro()
    {
        if(JoystickShoot.Vertical != 0  || JoystickShoot.Horizontal != 0 )
        {
            if(velRecarga <= 0) cargaTiro += Time.deltaTime;
        }
        else
        {
            cargaTiro = 0;
        }
    }

    void WeaponAspect()
    {
        gameObject.GetComponent<MeshFilter>().mesh = ActualWeapon.mesh;
        gameObject.GetComponent<MeshRenderer>().material = ActualWeapon.material;
    }

    /*void BarraCargaPosition()
    {
        barraBala.GetComponent<Slider>().maxValue = ActualWeapon.tiempoCargaTiro;
        if (cargaTiro > 0 && ActualWeapon.tiempoCargaTiro > 0)
        {
            barraBala.SetActive(true);
        }
        else
        {
            barraBala.SetActive(false);
        }
        barraBala.transform.position = new Vector3(posBarras.position.x, posBarras.position.y + 2, posBarras.position.z);
        barraBala.GetComponent<Slider>().value = cargaTiro;
    }*/

    /*void BarraCargaCartucho()
    {
        BarraCartucho.GetComponent<Slider>().maxValue = ActualWeapon.tiempoRecarga;
        if(velRecarga > 0)
        {
            BarraCartucho.SetActive(true);
        }
        else
        {
            BarraCartucho.SetActive(false);
        }
        BarraCartucho.transform.position = new Vector3(posBarras.position.x, posBarras.position.y + 2, posBarras.position.z);
        BarraCartucho.GetComponent<Slider>().value = velRecarga;
    }*/

    void SeleccionarArma()
    {
        ActualWeapon = Weapons[Singleton.Instance.ActualWeapon];
    }

    void ChangeWepon()
    {
        if(Singleton.Instance.ChangeWepon)
        {
            activeBullets = Weapons[Singleton.Instance.ActualWeapon].tamañoCartucho;
            velRecarga = 0;
            ibullet = 0;
        }
    }
}
