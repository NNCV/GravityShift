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
    public GameObject explosion;
    public Transform healingOrb, damagingOrb;
    public Material healMat, dmgMat;
    public Material expMat;

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

            if (healTC >= healTM)
            {
                GameObject hlWv = healWave;
                hlWv.GetComponent<HealWaveScript>().mat = new Material(healMat);
                Instantiate(hlWv, healingOrb.transform.position, healingOrb.transform.rotation);
                healTC = 0f;
            }
        }
        else;
        {
            Explode();
        }
    }

    public override void Explode()
    {
        GameObject exp = explosion;
        exp.GetComponent<ExplosionScript>().mat = new Material(expMat);
        exp.GetComponent<ExplosionScript>().scaleInit = 0f;
        exp.GetComponent<ExplosionScript>().scaleMax = 16f;
        exp.GetComponent<ExplosionScript>().distInit = 85f;
        exp.GetComponent<ExplosionScript>().distFin = 75f;
        exp.GetComponent<ExplosionScript>().speed = 1.5f;
        exp.GetComponent<ExplosionScript>().speedInit = new Vector2(4f, 4f);
        exp.GetComponent<ExplosionScript>().speedFin = new Vector2(1f, 1f);
        exp.GetComponent<ExplosionScript>().initFL = -6f;
        exp.GetComponent<ExplosionScript>().finalFL = 1f;
        Instantiate(exp, transform.position + new Vector3(0f, 0f, 0.01f), transform.rotation);
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
        base.Explode();
    }

}
