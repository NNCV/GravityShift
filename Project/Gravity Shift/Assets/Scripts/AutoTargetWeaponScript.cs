using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetWeaponScript : RotatingWeaponScript {
    
    public bool isFiring = false;
    public float salvoRange = 12f;
    public float damage = 0.2f;
    public LineRenderer laserBeam;

    public override void Fire()
    {
        base.Fire();

        isFiring = !isFiring;
        weaponMuzzleFlash.Emit(muzzleFlashParticleCount);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        /*
        if(isFiring)
        {
            Ray r = new Ray(weaponSalvoOrigin.position, weaponSalvoOrigin.up);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, salvoRange))
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    float dist = Vector2.Distance(weaponSalvoOrigin.position, hit.collider.gameObject.transform.position);
                    Debug.Log("NICE, HIT THE " + hit.collider.gameObject.name + " WHILE BEING JUST " + dist + " UNITS AWAY");
                    weaponMuzzleFlash.Emit(1);

                    laserBeam.SetPosition(0, weaponSalvoOrigin.position);
                    laserBeam.SetPosition(0, hit.point);

                    laserBeam.gameObject.SetActive(true);

                    if (hit.collider.gameObject.GetComponent<BasicEnemyScript>().shieldCurrent > 0)
                    {
                        if (hit.collider.gameObject.GetComponent<BasicEnemyScript>().shieldCurrent - damage >= 0)
                        {
                            hit.collider.gameObject.GetComponent<BasicEnemyScript>().shieldCurrent -= damage;
                        }
                        else
                        {
                            float hullDamage = damage - hit.collider.gameObject.GetComponent<BasicEnemyScript>().shieldCurrent;
                            hit.collider.gameObject.GetComponent<BasicEnemyScript>().shieldCurrent -= damage - hullDamage;
                            hit.collider.gameObject.GetComponent<BasicEnemyScript>().healthCurrent -= hullDamage;
                        }
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<BasicEnemyScript>().healthCurrent -= damage;
                    }
                }

              //  pm.pem.energyCurrent -= energyDrain;
              //  pm.pem.energyCooldown = 0;
            }
            else
            {
                isFiring = false;
                laserBeam.gameObject.SetActive(false);
            }
        }*/
    }

}
