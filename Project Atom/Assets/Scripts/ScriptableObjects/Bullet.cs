using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bullet : ScriptableObject
{
    public string bulletName;
    public bool enemyBullet;
    public float speed;
    public float upForce;
    public float knockback;
    public float damage;
    public float lifeTime;
    public bool bounce;
    public bool useGravity;
    [Range(0, .1f)] public float gravityForce;
    [Range(0,1)]public float recoil;
    public bool follow;
    public float rangeFollow;

    public Mesh mesh;
    public Material material;
    public TrailRenderer trail;
    public Material trailMaterial;
    public float hitRadio;

    public AudioClip sound;
}
