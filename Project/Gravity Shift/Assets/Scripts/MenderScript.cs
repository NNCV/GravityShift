using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenderScript : BasicEnemyScript {
    
    public Rigidbody2D rb2d;
    public float offsetMultiplier;
    public float moveSpeed;
    public float rotSpeed;
    public float distMin;
    public float distMax;
    public float healTC, healTM;
    public float healingPower;
    public float damageTC, damageTM;
    public float damagePower;
    public GameObject healWave;
    public GameObject damageWave;
    public Transform healingOrb, damagingOrb;
    public Material healMat, dmgMat;

    public override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        healWave.GetComponent<HealWaveScript>().mat = healMat;
    }

    public override void FixedUpdate () {

        base.FixedUpdate();

        if (!dead)
        {
            if (Vector2.Distance(player.position, transform.position) >= distMin)
            {
                Vector2 dir = (Vector2)(player.position - transform.position);

                dir.Normalize();

                float rotateBy = Vector3.Cross(dir, transform.up).z;

                rb2d.velocity = transform.up * moveSpeed;
                rb2d.angularVelocity = -rotateBy * rotSpeed;
            }
            else
            {
                damageTC += Time.deltaTime;

                if (damageTC >= damageTM)
                {
                    GameObject dmgWv = damageWave;
                    dmgWv.GetComponent<DamageWaveScript>().mat = new Material(dmgMat);
                    Instantiate(dmgWv, damagingOrb.transform.position, damagingOrb.transform.rotation);
                    damageTC = 0f;
                }
            }

            healTC += Time.deltaTime;

            if(healTC >= healTM)
            {
                GameObject hlWv = healWave;
                hlWv.GetComponent<HealWaveScript>().mat = new Material(healMat);
                Instantiate(hlWv, healingOrb.transform.position, healingOrb.transform.rotation);
                healTC = 0f;
            }
        }
    }
}
