using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    public PlayerManager pm;
    public PPLHelperMover pplhm;

    public Text level;

    public GameObject sh, hp, en, xp, shEq, hpEq, enEq;

    public float curSh, curHp, curEn, curXp;

    public Animator anim;
    public int animState = 0;
    
    public void SetStateOfPPLHM(Transform tr)
    {
        pplhm.isInCutscene = !pplhm.isInCutscene;
        pplhm.posToGo = tr;
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            animState = -1;
            pm.stopTime();
        }

        anim.SetInteger("State", animState);

        curSh = (float)pm.pem.shieldCurrent / (float)pm.pem.shieldMax;
        curHp = (float)pm.pem.hullCurrent / (float)pm.pem.hullMax;
        curEn = (float)pm.pem.energyCurrent / (float)pm.pem.energyMax;

        sh.GetComponent<RectTransform>().localScale = new Vector3(curSh, 1f, 1f);
        hp.GetComponent<RectTransform>().localScale = new Vector3(curHp, 1f, 1f);
        en.GetComponent<RectTransform>().localScale = new Vector3(curEn, 1f, 1f);

        shEq.GetComponent<RectTransform>().localScale = new Vector3(curSh, 1f, 1f);
        hpEq.GetComponent<RectTransform>().localScale = new Vector3(curHp, 1f, 1f);
        enEq.GetComponent<RectTransform>().localScale = new Vector3(curEn, 1f, 1f);

        level.text = pm.playerLevel.ToString();
    }

    public void SetAnimState(int a)
    {
        animState = a;
    }
}
