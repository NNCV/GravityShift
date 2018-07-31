using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveObject : ScriptableObject {

    public string objectiveType;
    public string objectiveText;
    public int objectiveProgress;
    public int objectiveRequired;
    public bool objectiveDone;
    public int objectiveXP;
    public GeneralItem[] objectiveItems = new GeneralItem[0];
    public ObjectiveObject objectiveNext;
    public Sprite objectiveImage;
    public string objectiveDoneText;
    public bool isExtra;

    public virtual void checkIfComplete()
    {
        if(objectiveProgress >= objectiveRequired)
        {
            objectiveDone = true;

        }
    }

    public virtual void giveReward()
    {
        if(objectiveItems.Length > 0)
        {
            foreach(GeneralItem item in objectiveItems)
            {
                FindObjectOfType<PlayerInventoryManager>().AddItemToLastPosition(item);
            }
        }

        FindObjectOfType<PlayerManager>().currentXP += objectiveXP;

        if(objectiveNext != null)
        {
            FindObjectOfType<ObjectiveManager>().currentObjectives.Add(objectiveNext);
        }
    }

}
