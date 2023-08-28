using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnswerTypes
{
    nextReplica = 0,
    exit = 1,
    shop = 2,
    healhpAndFinish = 3,
    healmpAndFinish = 4,
    healAllAndFinish = 5,
    giveItemAndFinish = 6,
    ToTheNextLevel = 7,
    OpenDoor = 8,
    Boss = 9,
    finish = 10

}
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(2, 5)]
    public string dialogueDescription;
    public string npcName;
    public Sprite NpcImage;
    public Replicas[] replicas;
}
[System.Serializable]
public struct Replicas
{
    [TextArea(2, 5)]
    public string[] replicaText;
    public Answers[] answers;
    [System.Serializable]
    public struct Answers
    {
        public string answerText;
        public AnswerTypes answerType;
        public int links;
    }
}
