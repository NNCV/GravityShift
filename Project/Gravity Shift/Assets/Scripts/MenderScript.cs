using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenderScript : BasicEnemyScript {

    public Transform player;
    public Rigidbody2D rb2d;
    public float offsetMultiplier;
    public float moveSpeed;
    public float rotSpeed;
    public float distMin;
    public float distMax;

    public override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate () {

        base.FixedUpdate();

        if (!dead)
        {
            Vector2 dir = (Vector2)(player.position - transform.position);
            
            dir.Normalize();

            float rotateBy = Vector3.Cross(dir, transform.up).z;
            
            if (Vector2.Distance(player.position, transform.position) >= distMin)
            {
                rb2d.angularVelocity = -rotateBy * rotSpeed;
            }
            else
            {
                rb2d.angularVelocity = rotateBy * rotSpeed * offsetMultiplier;
            }

            rb2d.velocity = transform.up * moveSpeed;
        }
    }
}
