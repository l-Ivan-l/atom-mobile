using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CloseWeapon : ScriptableObject
{
    public string weponName;
    public string description;
    public float damage;
    public float knockback;
    public float StartTimeBtwAtack;
    public Vector3 posCollision;
    public Vector3 atackRange;

    [Header("esthetic")]
    public Mesh mesh;
    public Material material;
}
