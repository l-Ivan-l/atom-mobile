using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton
{
    private static Singleton instance;
    private int actualWeapon ;
    private int actualBullet ;
    private int weaponType;
    private bool atack;
    private bool canMove;
    private float stopTime;
    private float playerLife = 450;
    private float currentBossLife;
    private bool changeWeapon = false;

    private Singleton() { } //Constructor privado.

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public int ActualWeapon
    {
        get { return actualWeapon; }
        set { actualWeapon = value; }
    }

    public int ActualBullet
    {
        get { return actualBullet; }
        set { actualBullet = value; }
    }

    public int WeaponType
    {
        get { return weaponType; }
        set { weaponType = value; }
    }

    public bool Atack
    {
        get { return atack; }
        set { atack = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public float StopTime
    {
        get { return stopTime; }
        set { stopTime = value; }
    }

    public bool ChangeWepon
    {
        get { return changeWeapon; }
        set { changeWeapon = value; }
    }

    public float PlayerLife
    {
        get { return playerLife; }
        set { playerLife = value; }
    }

    public float CurrentBossLife
    {
        get { return currentBossLife; }
        set { currentBossLife = value; }
    }
}
