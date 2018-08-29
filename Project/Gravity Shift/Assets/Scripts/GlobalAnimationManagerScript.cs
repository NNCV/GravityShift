using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAnimationManagerScript : MonoBehaviour {

    public Material m1, m2, m3, m4;
    [ColorUsageAttribute(true, true)]
    public Color c1from, c2from, c3from, c4from;
    public float distFrom;
    public Vector2 s1from, s2from, s3from, s4from;
    public Vector2 s1fromN, s2fromN, s3fromN, s4fromN;
    [ColorUsageAttribute(true, true)]
    public Color to1, to2, to3, to4;
    public float distTo;
    public Vector2 s1to, s2to, s3to, s4to;
    public Vector2 s1toN, s2toN, s3toN, s4toN;
    public SpriteRenderer srL;
    public float fromLspeed, toLspeed;
    public float fromLsize, toLsize;
    public Color fromL, toL;

    public PlayerManager pm;

    public float time;
    public bool destabilized = false;
    
    public void showEnemiesInboundDiagTEST()
    {
        pm.dm.ShowDialogue(pm.enemiesInboundDialogue);
    }

    public void showDiagTest()
    {
        FindObjectOfType<ObjectiveManager>().showInformationOfAllObjectives();
    }

    public void stopDiagWhileJumping()
    {
        FindObjectOfType<DialogueManager>().canDisplay = false;
        foreach (GameObject diag in GameObject.FindGameObjectsWithTag("DialogueBox"))
        {
            Destroy(diag);
        }
    }
    
    public void stopJump()
    {
        FindObjectOfType<ObjectiveManager>().jumpActivated = false;
    }

    public void startJump()
    {
        FindObjectOfType<ObjectiveManager>().jumpActivated = true;
    }

    public void startDiag()
    {
        FindObjectOfType<DialogueManager>().canDisplay = true;
    }

    public void setNormalTEST()
    {
        FindObjectOfType<PlayerUIManager>().animState = 98;
        pm.globalAnim.SetInteger("State", 0);
        pm.isFrozen = false;
        pm.isInCutscene = false;
        pm.warping = false;
        pm.tutorialIntroAnimation = false;
        pm.loadSector();
        FindObjectOfType<CameraMovementManager>().target = pm.gameObject;
        instantRestabilization();
    }

    public void Start()
    {
        pm = FindObjectOfType<PlayerManager>();
    }

    public void startTutorialDialogue()
    {
        pm.pem.showTutorialDialogue();
    }

    public void clearSectorParts()
    {
        foreach (GameObject oldSector in GameObject.FindGameObjectsWithTag("SectorGOPart"))
        {
            Destroy(oldSector);
        }
        
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (GameObject drop in GameObject.FindGameObjectsWithTag("Drop"))
        {
            Destroy(drop);
        }
    }
    
    public void Update()
    {
        if(destabilized == true)
        {
            destabilizeShift();
        }
        else
        {
            restabilizeShift();
        }
    }

    public void startDestabilizationShift()
    {
        destabilized = true;
    }

    public void returnToNormalShift()
    {
        destabilized = false;
    }
    
    public void instantRestabilization()
    {
        m1.SetColor("Color_40ED9A04", c1from);
        m2.SetColor("Color_40ED9A04", c2from);
        m3.SetColor("Color_40ED9A04", c3from);
        m4.SetColor("Color_40ED9A04", c4from);

        m1.SetVector("Vector2_9D1115A5", s1from);
        m2.SetVector("Vector2_9D1115A5", s2from);
        m3.SetVector("Vector2_9D1115A5", s3from);
        m4.SetVector("Vector2_9D1115A5", s4from);

        m1.SetVector("Vector2_E88029BF", s1fromN);
        m2.SetVector("Vector2_E88029BF", s2fromN);
        m3.SetVector("Vector2_E88029BF", s3fromN);
        m4.SetVector("Vector2_E88029BF", s4fromN);

        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierBefore = fromLspeed;
        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierAfter = fromLsize;
        srL.color = fromL;

        destabilized = false;
    }

    public void restabilizeShift()
    {
        m1.SetColor("Color_40ED9A04", Color.Lerp(m1.GetColor("Color_40ED9A04"), c1from, Time.deltaTime * time));
        m2.SetColor("Color_40ED9A04", Color.Lerp(m2.GetColor("Color_40ED9A04"), c2from, Time.deltaTime * time));
        m3.SetColor("Color_40ED9A04", Color.Lerp(m3.GetColor("Color_40ED9A04"), c3from, Time.deltaTime * time));
        m4.SetColor("Color_40ED9A04", Color.Lerp(m4.GetColor("Color_40ED9A04"), c4from, Time.deltaTime * time));

        m1.SetVector("Vector2_9D1115A5", Vector2.Lerp(m1.GetVector("Vector2_9D1115A5"), s1from, Time.deltaTime * time));
        m2.SetVector("Vector2_9D1115A5", Vector2.Lerp(m2.GetVector("Vector2_9D1115A5"), s2from, Time.deltaTime * time));
        m3.SetVector("Vector2_9D1115A5", Vector2.Lerp(m3.GetVector("Vector2_9D1115A5"), s3from, Time.deltaTime * time));
        m4.SetVector("Vector2_9D1115A5", Vector2.Lerp(m4.GetVector("Vector2_9D1115A5"), s4from, Time.deltaTime * time));

        m1.SetVector("Vector2_E88029BF", Vector2.Lerp(m1.GetVector("Vector2_E88029BF"), s1fromN, Time.deltaTime * time));
        m2.SetVector("Vector2_E88029BF", Vector2.Lerp(m2.GetVector("Vector2_E88029BF"), s2fromN, Time.deltaTime * time));
        m3.SetVector("Vector2_E88029BF", Vector2.Lerp(m3.GetVector("Vector2_E88029BF"), s3fromN, Time.deltaTime * time));
        m4.SetVector("Vector2_E88029BF", Vector2.Lerp(m4.GetVector("Vector2_E88029BF"), s4fromN, Time.deltaTime * time));

        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierBefore = Mathf.Lerp(srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierBefore, fromLspeed, Time.deltaTime * time);
        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierAfter = Mathf.Lerp(srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierAfter, fromLsize, Time.deltaTime * time);
        srL.color = Color.Lerp(srL.color, fromL, Time.deltaTime * time);
    }

    public void destabilizeShift()
    {
        m1.SetColor("Color_40ED9A04", Color.Lerp(m1.GetColor("Color_40ED9A04"), to1, Time.deltaTime * time));
        m2.SetColor("Color_40ED9A04", Color.Lerp(m2.GetColor("Color_40ED9A04"), to2, Time.deltaTime * time));
        m3.SetColor("Color_40ED9A04", Color.Lerp(m3.GetColor("Color_40ED9A04"), to3, Time.deltaTime * time));
        m4.SetColor("Color_40ED9A04", Color.Lerp(m4.GetColor("Color_40ED9A04"), to4, Time.deltaTime * time));

        m1.SetVector("Vector2_9D1115A5", Vector2.Lerp(m1.GetVector("Vector2_9D1115A5"), s1to, Time.deltaTime * time));
        m2.SetVector("Vector2_9D1115A5", Vector2.Lerp(m2.GetVector("Vector2_9D1115A5"), s2to, Time.deltaTime * time));
        m3.SetVector("Vector2_9D1115A5", Vector2.Lerp(m3.GetVector("Vector2_9D1115A5"), s3to, Time.deltaTime * time));
        m4.SetVector("Vector2_9D1115A5", Vector2.Lerp(m4.GetVector("Vector2_9D1115A5"), s4to, Time.deltaTime * time));

        m1.SetVector("Vector2_E88029BF", Vector2.Lerp(m1.GetVector("Vector2_E88029BF"), s1toN, Time.deltaTime * time));
        m2.SetVector("Vector2_E88029BF", Vector2.Lerp(m2.GetVector("Vector2_E88029BF"), s2toN, Time.deltaTime * time));
        m3.SetVector("Vector2_E88029BF", Vector2.Lerp(m3.GetVector("Vector2_E88029BF"), s3toN, Time.deltaTime * time));
        m4.SetVector("Vector2_E88029BF", Vector2.Lerp(m4.GetVector("Vector2_E88029BF"), s4toN, Time.deltaTime * time));
        
        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierBefore = Mathf.Lerp(srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierBefore, toLspeed, Time.deltaTime * time);
        srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierAfter = Mathf.Lerp(srL.gameObject.GetComponent<WarpConduitLensFlareScript>().multiplierAfter, toLsize, Time.deltaTime * time);
        srL.color = Color.Lerp(srL.color, toL, Time.deltaTime * time);
    }
}
