using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeaponScript : MonoBehaviour {

    public PlayerManager pm;
    public Transform weaponSalvoOrigin;
    public ParticleSystem weaponMuzzleFlash;
    public int muzzleFlashParticleCount;

    public float energyDrain, fireRate, coolDown;

    public float shakeXmin, shakeYmin, shakeXmax, shakeYmax;
    public float shakeZmin, shakeZmax;

    //All of these will be implemented differently depending on the
    //weapon type, weapon movement type, and many other factors

    public virtual void Fire() {}

    public virtual void Start()
    {
        pm = FindObjectOfType<PlayerManager>();
    }

    public virtual void FixedUpdate() {}

}
