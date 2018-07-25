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
    public Image jumpSystemTypeImage;
    public Text jumpSystemTypeText;
    public Text jumpSectorTypeText;
    public Sprite[] jumpSystemTypes;

    public bool jumped = true;
    public float timeCurrent, timeMax;
    
    public void spawnEN()
    {
        pm.spawnEnemy();
    }

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
            if (systemIN.systemOrbitStage == 0)
            {
                galaxyViewTexts[5].text = "This is the center";
            }
            else
            {
                galaxyViewTexts[5].text = systemIN.systemOrbitStage.ToString() + "00 light years";
            }
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
    
    public void setJumpSystemInformation(int sector)
    {
        string systemType = pm.currentGalaxy.systems[pm.currentSystem].systemType;

        string preFinalSystemText = "";
        string preFinalSectorText = "";

        if (systemType.Contains("nebula"))
        {
            jumpSystemTypeImage.sprite = jumpSystemTypes[2];
            preFinalSystemText = "nebula system " + pm.currentGalaxy.systems[pm.currentSystem].systemName;
        }
        else
        {
            jumpSystemTypeImage.sprite = jumpSystemTypes[0];
            preFinalSystemText = "system " + pm.currentGalaxy.systems[pm.currentSystem].systemName;
        }

        if (sector > 0)
        {
            preFinalSectorText = pm.currentGalaxy.systems[pm.currentSystem].systemPlanets[pm.currentSector].planetType + " " + pm.currentGalaxy.systems[pm.currentSystem].systemPlanets[pm.currentSector].sectorName;
        }

        string finalSystemText = "";
        string finalSectorText = "";

        foreach (char c in preFinalSystemText)
        {
            if (c == ' ')
            {
                finalSystemText += "   ";
            }
            else
            {
                finalSystemText += c + " ";
            }
        }

        foreach (char c in preFinalSectorText)
        {
            if (c == ' ')
            {
                finalSectorText += "   ";
            }
            else
            {
                finalSectorText += c + " ";
            }
        }

        finalSystemText = finalSystemText.Trim();
        finalSectorText = finalSectorText.Trim();

        jumpSystemTypeText.text = finalSystemText;
        jumpSectorTypeText.text = finalSectorText;
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && (pm.warping == false && pm.warmingUp == false))
        {
            animState = -1;
            pm.stopTime();
        }

        if(pm.warping == true || pm.warmingUp == true)
        {
            animState = 100;
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
