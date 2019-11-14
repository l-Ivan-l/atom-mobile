using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Gun : ScriptableObject
{
    public string gunName;
    public string description;
    public float fireRate;
    public int tamañoCartucho;
    public float tiempoRecarga;
    public float tiempoCargaTiro;
    public AudioClip rechargeSound;
    [Range(0,1)]public float volume;

    public Mesh mesh;
    public Material material;
}
