using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSettings : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue dialogEnd;

    public bool isShop;
    [Header("Debug")]
    public bool dialogueEnded;
    public bool dialogueStarted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (dialogueEnded && dialogEnd == null) return;
            collision.GetComponent<Interactive>().dialogue = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Interactive>().dialogue = null;  
        }
    }
}
