using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDamageHandler : MonoBehaviour
{
    float hp;
    void Start()
    {
        hp = GetComponent<Damageable>().health;
    }

    public void takeDamage()
    {
        print("OW");
    }

    public void die()
    {
        Lean.Pool.LeanPool.Despawn(this);
    }
}
