using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeaponScript : BasicWeaponScript {

    public float rotationSpeed;
    public float rotationOffsetZ;

    public override void Start()
    {
        base.Start();

        transform.rotation = Quaternion.Euler(0f, 0f, rotationOffsetZ);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 dif = new Vector3();

        if (!pm.warmingUp || !pm.warping || !pm.tutorialIntroAnimation)
        {
            if (pm.isFrozen == false || pm.isInCutscene == false)
            {
                dif = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dif.Normalize();
            }
            else
            {
                dif = new Vector3(0f, 1f, 0f);
            }

            float rotZ = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotZ + rotationOffsetZ), Time.deltaTime * rotationSpeed);
        }
    }

}
