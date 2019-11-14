using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void Hurt(float Damage);
    void EnableKnockback(Vector3 direction, float time, float force);
}
