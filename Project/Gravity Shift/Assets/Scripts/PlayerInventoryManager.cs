using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour {

    [System.Serializable]
    public class SlotLine
    {
        public SlotScript[] line;
    }

    public SlotLine[] slots;

    public void AddItemToLastPosition(GeneralItem gi, int a = 0, int b = 0)
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                if (slots[i].line[j].itemInSlot == null)
                {
                    slots[i].line[j].itemInSlot = gi;
                    slots[i].line[j].DisplayItem();
                    return;
                }
            }
        }
    }

    public void ClearInventory()
    {
        foreach(SlotLine sl in slots)
        {
            foreach(SlotScript ss in sl.line)
            {
                ss.itemInSlot = null;
                ss.DisplayItem();
            }
        }
    }

}
