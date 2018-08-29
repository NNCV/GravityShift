using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTargetWeaponScript : RotatingWeaponScript {

    public GameObject manualTargetProjectile;


    public override void Fire()
    {
        base.Fire();

        GameObject proj = manualTargetProjectile;
        Instantiate(proj, weaponSalvoOrigin.position, weaponSalvoOrigin.rotation);

        weaponMuzzleFlash.Emit(muzzleFlashParticleCount);

        float shakeAddX = Random.Range(shakeXmin, shakeXmax);
        float shakeAddY = Random.Range(shakeYmin, shakeYmax);
        float shakeRotZ = Random.Range(shakeZmin, shakeZmax);

        Camera.main.GetComponent<CameraMovementManager>().Shake(shakeAddX, shakeAddY, shakeRotZ);

        pm.pem.energyCurrent -= energyDrain;
        pm.pem.energyCooldown = 0;
    }
}
