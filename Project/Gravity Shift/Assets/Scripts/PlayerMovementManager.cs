using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovementManager : MonoBehaviour
{
    public float fSpeed = 1f;
    public float rSpeed = 1f;

    public float hydrSpeed = 1f;

    public Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (this.GetComponent<PlayerManager>().warping == true)
        {
            rb2d.velocity = new Vector3(0f, 0f, 0f);
            rb2d.angularVelocity = 0f;
        }
        else
        if (this.GetComponent<PlayerManager>().isFrozen == false)
        {
            rb2d.AddRelativeForce(new Vector2(0f, Input.GetAxisRaw("Vertical") * fSpeed * hydrSpeed));
            rb2d.AddTorque(-Input.GetAxis("Horizontal") * rSpeed);
        }
    }
}