using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    public PlayerManager pm;

    public Text level;

    public GameObject sh, hp, en, xp, shEq, hpEq, enEq;

    public float curSh, curHp, curEn, curXp;

    public Animator anim;
    public int animState = 0;

    public Text[] galaxyViewTexts;
    public Text[] systemViewTexts;
    public Text galaxySectorSwitchView;

    public void setGalaxyViewStats(SystemLevelObject systemIN)
    {
        if (systemIN != null)
        {
            foreach (Text tx in galaxyViewTexts)
            {
                tx.transform.gameObject.SetActive(true);
            }
            galaxyViewTexts[1].text = systemIN.systemName;
            galaxyViewTexts[3].text = (systemIN.systemPlanetCount - 1).ToString();
            galaxyViewTexts[5].text = systemIN.systemOrbitStage.ToString() + "00 light years";

        }
        else
        {
            disableGalaxyViewStats();
        }
    }

    public void disableGalaxyViewStats()
    {
        foreach(Text tx in galaxyViewTexts)
        {
            tx.transform.gameObject.SetActive(false);
        }
    }

    public void switchGalaxySystemText()
    {
        if(galaxySectorSwitchView.text == "galaxy view")
        {
            galaxySectorSwitchView.text = "system view";
        }
        else if (galaxySectorSwitchView.text == "system view")
        {
            galaxySectorSwitchView.text = "galaxy view";
        }
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

    public void SwitchMapNormalCameraControls()
    {
        Camera.main.GetComponent<CameraMovementManager>().isInMapScreen = !Camera.main.GetComponent<CameraMovementManager>().isInMapScreen;
    }

    public void SetAnimState(int a)
    {
        animState = a;
    }
}
