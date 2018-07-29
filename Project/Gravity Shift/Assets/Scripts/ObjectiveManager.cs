using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public DialogueManager dm;
    public DialogueScript ds;
    public DialogueScript dsActivatedJump;
    public List<ObjectiveObject> currentObjectives;
    public GameObject[] enemies;
    public DialogueObject noObjective;
    public DialogueObject jumpActivatedDialogue;
    public Sprite abrarum;
    public bool jumpActivated = false;

    public void setObjective(ObjectiveObject ooIN, int pos)
    {
        currentObjectives[pos] = ooIN;
    }

    public DialogueObject getObjectiveInformation(ObjectiveObject ooShow, int pos)
    {
        DialogueObject objectiveDialogue = new DialogueObject();
        objectiveDialogue.characterImage = ooShow.objectiveImage;
        objectiveDialogue.characterText = ooShow.objectiveText + "\n" + ooShow.objectiveProgress + "/" + ooShow.objectiveRequired;
        objectiveDialogue.characterName = "objective " + (pos + 1);
        objectiveDialogue.timeToDisappear = 2f;
        objectiveDialogue.isSkippable = true;
        ds.canBeOverlayed = true;
        ds.canBeSkippedEntirely = true;
        return objectiveDialogue;
    }
    
    //Functie ce afiseaza toate obiectivele cu ajutorul dialogue-manager-ului
    public void showInformationOfAllObjectives()
    {
        if (currentObjectives.Count > 0)
        {
            ds.dialogueLines = new DialogueObject[currentObjectives.Count + 1];
            DialogueObject[] allObjInfo = new DialogueObject[currentObjectives.Count + 1];

            for (int a = 0; a < currentObjectives.Count; a++)
            {
                allObjInfo[a] = getObjectiveInformation(currentObjectives[a], a);
            }

            allObjInfo[currentObjectives.Count] = getObjectiveInformation(currentObjectives[currentObjectives.Count - 1], currentObjectives.Count - 1);

            ds.dialogueLines = allObjInfo;
            dm.ShowDialogue(ds);
        }
        else
        {
            ds.dialogueLines = new DialogueObject[2];
            ds.dialogueLines[0] = noObjective;
            ds.dialogueLines[1] = noObjective;

            dm.ShowDialogue(ds);
        }
    }

    public void Update()
    {
        bool onlyExtras = false;
        foreach(ObjectiveObject oobj in currentObjectives)
        {
            if(oobj.isExtra == true)
            {
                onlyExtras = true;
            }
        }
        if (currentObjectives.Count > 0)
        {
            for (int a = 0; a < currentObjectives.Count; a++)
            {
                if (currentObjectives[a] != null)
                {
                    if (currentObjectives[a].objectiveProgress >= currentObjectives[a].objectiveRequired)
                    {
                        currentObjectives[a].objectiveDone = true;
                    }
                    if (currentObjectives[a].objectiveDone == true)
                    {
                        DialogueObject objectiveDone = new DialogueObject();
                        objectiveDone.characterImage = currentObjectives[a].objectiveImage;
                        objectiveDone.characterName = "objective " + (a + 1) + " completed";
                        if (currentObjectives[a].objectiveDoneText == null)
                        {
                            objectiveDone.characterText = "objective number " + a + " completed!\n jump drive fully charged!";
                        }
                        else
                        {
                            objectiveDone.characterText = currentObjectives[a].objectiveDoneText;
                        }

                        objectiveDone.timeToDisappear = 2f;
                        currentObjectives[a].giveReward();

                        ds.dialogueLines = new DialogueObject[2];
                        ds.dialogueLines[0] = objectiveDone;
                        ds.dialogueLines[1] = objectiveDone;
                        dm.ShowDialogue(ds);

                        currentObjectives[a] = null;

                        List<ObjectiveObject> newObjList = new List<ObjectiveObject>();

                        foreach (ObjectiveObject obj in currentObjectives)
                        {
                            if (obj != null)
                            {
                                newObjList.Add(obj);
                            }
                        }

                        currentObjectives = newObjList;
                    }
                }
            }
        }
        else if(jumpActivated == false || onlyExtras == true)
        {
            jumpActivated = true;

            FindObjectOfType<PlayerManager>().canJump = true;
            
            dsActivatedJump.dialogueLines = new DialogueObject[2];
            dsActivatedJump.dialogueLines[0] = jumpActivatedDialogue;
            dsActivatedJump.dialogueLines[1] = jumpActivatedDialogue;

            dm.ShowDialogue(dsActivatedJump);
        }
    }
}
