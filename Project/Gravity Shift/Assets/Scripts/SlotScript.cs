using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour {

    public GeneralItem itemInSlot;
    public string slotType;

    public Image itemImage;

    public int x = 0;
    public int y = 0;

    public SlotDialogueScript sds;
    public Sprite defaultSprite;

    private void Start()
    {
        if (itemInSlot == null)
        {
            itemImage.sprite = defaultSprite;
        }
        else
        if (itemInSlot.itemType == "General" || itemInSlot.itemType == "Quest")
        {
            itemImage.sprite = itemInSlot.itemImage;
        }
        else if (itemInSlot.itemName == "Hellstorm")
        {
            //EquippableItem eqi = (EquippableItem)itemInSlot;
            //itemImage.sprite = eqi.slotImage;
        }
        else
        {
            EquippableItem eqi = (EquippableItem)itemInSlot;
            itemImage.sprite = eqi.slotImage;
        }
    }

    public void DisplayItem()
    {
        if (itemInSlot == null)
        {
            itemImage.sprite = defaultSprite;
        }
        else
        if (itemInSlot.itemType == "General" || itemInSlot.itemType == "Quest")
        {
            itemImage.sprite = itemInSlot.itemImage;
        }
        else
        {
            EquippableItem eqi = (EquippableItem)itemInSlot;
            itemImage.sprite = eqi.slotImage;
        }
    }

    public void DisableChildren()
    {
        for(int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(false);
        }
        transform.gameObject.SetActive(false);
    }

    public void DisplayDialogue()
    {
        if (itemInSlot != null)
        {
            switch (itemInSlot.itemType)
            {
                case "General":
                    sds.animState = 1;
                    sds.DisplayStats(this);
                    break;
                case "Quest":
                    sds.animState = 1;
                    sds.DisplayStats(this);
                    break;
                case "Equippable Shield":
                    sds.animState = 2;
                    sds.DisplayStats(this);
                    break;
                case "Weapon":
                    sds.animState = 3;
                    sds.DisplayStats(this);
                    break;
            }
            if(itemInSlot.itemType.Contains("Equippable"))
            {
                if (itemInSlot.itemType == "Equippable Weapon")
                {
                    sds.animState = 3;
                }
                else
                {
                    sds.animState = 2;
                }
                sds.DisplayStats(this);
            }
        }
        else sds.animState = 0;
    }
    
    public void StopDisplayingDialogue()
    {
        sds.animState = 0;
    }
}
