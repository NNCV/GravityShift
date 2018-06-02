using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotDialogueScript : MonoBehaviour {

    public float timeToFade = 0.25f;
    public float timeCurrent = 0f;

    public Animator anim;
    public int animState = 0;

    public SlotScript ss;

    //General Items (ships, weapons, shields, quest items, resources, etc)
    public Text itemNameBox;
    public Image itemImageBox;
    public Text itemTypeBox;
    public Text itemDescBox;

    //Equippable Items (ships, hulls, reactors, shields, weapons, etc)
    public Text eqStat1;
    public Text eqStat2;
    public Image eqStat1Current;
    public Image eqStat2Current;
    public Text eqDescBox;

    //Weapon Items (manual weapons, drones, etc)
    public Image wepReloadCurrent;
    public Image wepRangeCurrent;
    public Image wepPSpeedCurrent;
    public Image wepEnCurrent;
    public Text wepDescBox;
    public Text wepOCBox;

    public float requiredX = 2.5f;
    public float requiredY = 2.5f;

	void Start () {
		
	}
	
	void Update () {
        anim.SetInteger("State", animState);
        this.GetComponent<RectTransform>().anchoredPosition = ss.GetComponent<RectTransform>().anchoredPosition - new Vector2(requiredX, requiredY);
    }

    public void DisplayStats(SlotScript ssIN)
    {
        ss = ssIN;
        switch(animState)
        {
            case 1:
                itemNameBox.text = ss.itemInSlot.name;
                itemTypeBox.text = ss.itemInSlot.itemType;
                itemImageBox.sprite = ss.itemInSlot.itemImage;
                itemDescBox.text = ss.itemInSlot.itemDescription;
                break;
            case 2:
                itemNameBox.text = ss.itemInSlot.name;
                itemTypeBox.text = ss.itemInSlot.itemType;
                itemDescBox.text = ss.itemInSlot.itemDescription;
                EquippableItem eq = (EquippableItem)ss.itemInSlot;
                itemImageBox.sprite = eq.slotImage;
                eqStat1.text = eq.stat1;
                eqStat1Current.rectTransform.localScale = new Vector3(eq.stat1Value, 1f, 1f);
                eqStat2.text = eq.stat2;
                eqStat2Current.rectTransform.localScale = new Vector3(eq.stat2Value, 1f, 1f);
                eqDescBox.text = eq.itemDescription;
                break;
            case 3:
                itemNameBox.text = ss.itemInSlot.name;
                itemTypeBox.text = ss.itemInSlot.itemType;
                itemDescBox.text = ss.itemInSlot.itemDescription;
                EquippableItem eq2 = (EquippableItem)ss.itemInSlot;
                itemImageBox.sprite = eq2.slotImage;
                eqStat1.text = eq2.stat1;
                eqStat1Current.rectTransform.localScale = new Vector3(eq2.stat1Value, 1f, 1f);
                eqStat2.text = eq2.stat2;
                eqStat2Current.rectTransform.localScale = new Vector3(eq2.stat2Value, 1f, 1f);
                eqDescBox.text = eq2.itemDescription;
                BlasterItem bi = (BlasterItem)ss.itemInSlot;
                wepReloadCurrent.rectTransform.localScale = new Vector3(1f, bi.wep1Value, 1f);
                wepRangeCurrent.rectTransform.localScale = new Vector3(1f, bi.wep2Value, 1f);
                wepPSpeedCurrent.rectTransform.localScale = new Vector3(1f, bi.wep3Value, 1f);
                wepEnCurrent.rectTransform.localScale = new Vector3(1f, bi.wep4Value, 1f);
                wepDescBox.text = bi.itemDescription;
                wepOCBox.text = bi.overchargeEffect;
                break;
        }
    }

}
