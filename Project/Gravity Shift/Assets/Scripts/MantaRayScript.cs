using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantaRayScript : BasicEnemyScript {
    
    //Variabile pentru foc
    public GameObject projectile;
    public float angleMin, angleMax;
    public float distance;
    public float timeFireCurrent;
    public float timeFireRate;
    public Transform[] turrets;
    public int currentTurret = 0;

    //Variabile de miscare
    public float moveSpeed;
    public float rotSpeed;
    public Rigidbody2D rb2d;

    //Variabile pentru efectul de explozie
    public float offsetPosMin, offsetPosMax;
    public float offsetAngleMin, offsetAngleMax;
    public float offsetAngle;
    public Quaternion offsetRot;
    public Vector3 offsetPos;
    public float expSpeed;
    public bool offset = false;
    public float expTimeC, expTimeM;
    public GameObject explosion;
    public Material expMat;

    //Variabile pentru efectul de salt
    public Animator globalAnim;

    public override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate()
    {
        //playstate -1 : in animatia de inceput
        //playstate 0 : nava se misca si incepe sa urmareasca player-ul
        switch(playState)
        {
            case 0:
                base.FixedUpdate();
                if (!dead)
                {
                    //se calculeaza directia catre jucator dupa care se aplica 
                    Vector2 dir = (Vector2)(player.position - transform.position);
                    dir.Normalize();

                    float rotateBy = Vector3.Cross(dir, transform.up).z;

                    rb2d.angularVelocity = -rotateBy * rotSpeed;
                    rb2d.velocity = transform.up * moveSpeed;


                    //foc automat daca jucatorul este in raza si daca este vazut aproape direct
                    timeFireCurrent += Time.deltaTime;
                    if (timeFireCurrent >= timeFireRate)
                    {
                        timeFireCurrent = timeFireRate;
                        if (player != null)
                        {
                            Vector3 direction = player.position - transform.position;

                            float angle = Vector3.Angle(direction, transform.up);

                            if (angle >= angleMin && angle <= angleMax)
                            {
                                float dist = Vector2.Distance(transform.position, player.position);
                                if (dist <= distance)
                                {
                                    Shoot(currentTurret);
                                    if (currentTurret == 0)
                                        currentTurret = 1;
                                    else currentTurret = 0;
                                    timeFireCurrent = 0;
                                }
                            }

                        }
                    }
                }
                //se calculeaza anumite variabile pentru efectul de explozie si se insantiaza niste explozii si efecte
                else if (dead == true && offset == false)
                {
                    offsetAngle = Random.Range(offsetAngleMin, offsetAngleMax);
                    if ((int)Random.Range(0, 1) == 1)
                    {
                        offsetAngle = -offsetAngle;
                    }
                    offsetPos = new Vector3(Random.Range(offsetPosMin, offsetPosMax), Random.Range(offsetPosMin, offsetPosMax), 0f);
                    offsetPos += transform.position;
                    offsetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + offsetAngle, transform.rotation.w);
                    offset = true;
                }
                //se efectueaza efectul de explozie
                //se inlatura collider-ul si rigidbody-ul pentru a se misca bazat pe transform-ul legat
                //se asteapta pana putin timp dupa care obiectul explodeaza
                else if (dead == true && offset == true)
                {
                    Destroy(rb2d);
                    Destroy(GetComponent<BoxCollider2D>());
                    healthCurrent = 0;
                    shieldCurrent = 0;

                    transform.rotation = Quaternion.Lerp(transform.rotation, offsetRot, Time.deltaTime * expSpeed);
                    transform.position = Vector3.Lerp(transform.position, offsetPos, Time.deltaTime * expSpeed);

                    expTimeC += Time.deltaTime;

                    if (expTimeC >= expTimeM)
                    {
                        Explode();
                    }

                }
                break;
        }
        
    }

    //Functia folosita de inamic pentru a trage catre jucator
    void Shoot(int turret)
    {
        Instantiate(projectile, turrets[turret].transform.position, turrets[turret].transform.rotation);
    }

    //Instantiaza un efect de explozie si dupa care se distruge nava 
    public override void Explode()
    {
        GameObject exp = explosion;
        exp.GetComponent<ExplosionScript>().mat = new Material(expMat);
        exp.GetComponent<ExplosionScript>().scaleInit = 0f;
        exp.GetComponent<ExplosionScript>().scaleMax = 4f;
        exp.GetComponent<ExplosionScript>().distInit = 85f;
        exp.GetComponent<ExplosionScript>().distFin = 75f;
        exp.GetComponent<ExplosionScript>().speed = 2.5f;
        exp.GetComponent<ExplosionScript>().speedInit = new Vector2(4f, 4f);
        exp.GetComponent<ExplosionScript>().speedFin = new Vector2(1f, 1f);
        exp.GetComponent<ExplosionScript>().initFL = -6f;
        exp.GetComponent<ExplosionScript>().finalFL = 1f;
        Instantiate(exp, transform.position + new Vector3(0f, 0f, 0.01f), transform.rotation);
        base.Explode();
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
    }
}
