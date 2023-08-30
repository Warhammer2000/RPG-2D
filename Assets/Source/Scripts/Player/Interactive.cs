using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Interactive : MonoBehaviour
{
    public ItemSettings item;
    public Inventory inv;
    public DialogueSettings dialogue;
    public AudioSource source;
    [Inject] DialogueManager dialogueManager;
    private void Awake()
    {
        source = GetComponent<AudioSource>();   
    }
    void Start() {  inv = GetComponent<Inventory>(); }
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
                dialogueManager.StartDialogue(dialogue);
                dialogue = null;
            }
        }
    }

    public void TakeItem()
    {
        if (item != null) 
        {
            if (item.thisItem.myType == Item.ItemTypes.gold)
            {
                inv.AddGold(item.count);
                Destroy(item.gameObject);
            }
            if (inv.AddItem(item.thisItem, item.count))
            {
                source.PlayOneShot(item.thisItem.clip);
                Debug.Log("You catch up  item");
                Destroy(item.gameObject);
                item = null;
            }
            else { Debug.Log("Error"); }
        } 
    }


    private void TakeGold()
    {
        if (item != null)
        {
            source.PlayOneShot(item.thisItem.clip);
            Debug.Log("You catch up  Gold");
            inv.AddGold(item.count);
            Destroy(item.gameObject);
        }
    }
}
