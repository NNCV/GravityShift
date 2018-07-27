using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    public DialogueManager dm;
    public DialogueScript ds;
    public List<ObjectiveObject> currentObjectives;
    public GameObject[] enemies;

    public void setObjective(ObjectiveObject ooIN, int pos)
    {
        currentObjectives[pos] = ooIN;
    }

    public DialogueObject getObjectiveInformation(ObjectiveObject ooShow, int pos)
    {
        DialogueObject objectiveDialogue = new DialogueObject();
        objectiveDialogue.characterImage = ooShow.objectiveImage;
        objectiveDialogue.characterText = ooShow.objectiveText + "\n" + ooShow.objectiveProgress + "/" + ooShow.objectiveRequired;
        objectiveDialogue.characterName = "Objective " + pos;
        objectiveDialogue.timeToDisappear = 2f;
        objectiveDialogue.isSkippable = true;
        ds.canBeOverlayed = true;
        ds.canBeSkippedEntirely = true;
        return objectiveDialogue;
    }

    public void showInformationOfAllObjectives()
    {
        DialogueObject[] allObjInfo = new DialogueObject[currentObjectives.Count + 1];

        for (int a = 0; a < currentObjectives.Count; a++)
        {
            if (a == 0)
            {
                allObjInfo[a] = getObjectiveInformation(currentObjectives[a], a);
                allObjInfo[a + 1] = getObjectiveInformation(currentObjectives[a], a);
                a++;
            }
        }
    }

    public void Update()
    {
        for (int a = 0; a < currentObjectives.Count; a++)
        {
            if (currentObjectives[a].objectiveDone == true)
            {
                DialogueObject objectiveDone = new DialogueObject();
                objectiveDone.characterImage = currentObjectives[a].objectiveImage;
                if (currentObjectives[a].objectiveDoneText == null)
                {
                    objectiveDone.characterText = "Objective number " + a + " completed!\n Jump drive fully charged!";
                }
                else
                {
                    objectiveDone.characterText = currentObjectives[a].objectiveDoneText;
                }

                currentObjectives[a].giveReward();

                currentObjectives[a] = null;
                
                //Sort
                for (int b = currentObjectives.Count - 1; b > a; b--)
                {
                    currentObjectives[b - 1] = currentObjectives[b];
                }
                
            }
        }
    }
}
