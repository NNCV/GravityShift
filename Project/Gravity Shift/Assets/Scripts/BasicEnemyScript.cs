using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour {

    public float healthCurrent, healthMax;
    public float shieldCurrent, shieldMax;
    public bool dead = false;

    public virtual void Start()
    {
        healthCurrent = healthMax;
        shieldCurrent = shieldMax;
    }

    public virtual void FixedUpdate()
    {
        if(healthCurrent <= 0)
        {
            dead = true;
        }
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }
}
