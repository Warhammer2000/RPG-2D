using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemSettings item;
    public static Interactive player;
    public Inventory inv;
    public DialogueSettings dialogue;
    public AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();   
    }
    void Start() { player = this; inv = GetComponent<Inventory>(); }
    void Update()
    {
        if (item != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (item.thisItem.myType == Item.ItemTypes.gold) TakeGold();
                else TakeItem();
            }
        }
        else if(dialogue != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogue.dialogueStarted)
            {
                DialogueManager.Instance.StartDialogue(dialogue);
                dialogue = null;
            }
        }
    }

    private void TakeItem()
    {
        if(inv.AddItem(item.thisItem, item.count))
        {
            source.PlayOneShot(item.thisItem.clip);
            Debug.Log("You catch up  item");
            Destroy(item.gameObject);
            item = null;
        }
        else { Debug.Log("Error"); }
    }


    private void TakeGold()
    {
        source.PlayOneShot(item.thisItem.clip);
        Debug.Log("You catch up  Gold");
        inv.AddGold(item.count);
        Destroy(item.gameObject);
    }
}
