using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    public ShootScript wepon;
    public PlayerMovement playerLife;
    public Image weaponHUD;
    public Image FillCharger;
    public Image FillChargeShoot;
    public Image img_PlayerLife;
    public List<Sprite> Weapons;
    public List<Sprite> Bullets;
    public GameObject chargueShootObject;
    //public Animator animChargeShootBar;
    public Animator anim_Life;
    
    public GameObject bossBar;


    // Update is called once per frame
    void Update()
    {
        ChargerBar();
        ChargeShoot();
        ActualizarArma();
        ActualizarVida();
    }


    void ChargeShoot()
    {
        if (wepon.Weapons[Singleton.Instance.ActualWeapon].tiempoCargaTiro > 0)
        {
            //animChargeShootBar.SetBool("Show",true);
            //animChargeShootBar.SetBool("Hide", false);
            chargueShootObject.SetActive(true);
            FillChargeShoot.fillAmount = wepon.cargaTiro / wepon.Weapons[Singleton.Instance.ActualWeapon].tiempoCargaTiro;
        }
        else
        {
            chargueShootObject.SetActive(false);
            //    animChargeShootBar.SetBool("Hide", true);
            //    animChargeShootBar.SetBool("Show", false);

        }
            

    }
    void ChargerBar()
    {
        FillCharger.fillAmount = (float)wepon.activeBullets / (float)wepon.Weapons[Singleton.Instance.ActualWeapon].tamañoCartucho;
        if(wepon.activeBullets <= 0)
        {         
            FillCharger.fillAmount = wepon.velRecarga / wepon.Weapons[Singleton.Instance.ActualWeapon].tiempoRecarga;
        }
       
    }
    void ActualizarArma()
    {
        weaponHUD.sprite = Weapons[Singleton.Instance.ActualWeapon];
    }
    void ActualizarVida()
    {
        Color VerdeAzul = new Color(0.25f, 0.87f, 0.58f);
        Color RojoMuerte = new Color(0.8f, 0.2f, 0.0f);
        anim_Life.SetFloat("animSpeed", 1f);
        
        img_PlayerLife.color = Color.Lerp(RojoMuerte, VerdeAzul, Singleton.Instance.PlayerLife / playerLife.vida);
        img_PlayerLife.fillAmount = Singleton.Instance.PlayerLife / playerLife.vida;
        float velextra = 1 - Singleton.Instance.PlayerLife / playerLife.vida;
        anim_Life.SetFloat("animSpeed", 1f + velextra);
    }
    public void ShowBossLife()
    {
      bossBar.SetActive(true);
    }
}
