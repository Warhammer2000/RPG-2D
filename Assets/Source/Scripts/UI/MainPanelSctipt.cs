using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelSctipt : MonoBehaviour
{
    private StatsUi stats;
    private InventoryUI inventory;
    private SpellBookUI spellBook;
    public GameObject[] Panel;
    public void Access()
    {
        Panel = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            Panel[i] = transform.GetChild(i).gameObject;
        }
        inventory = Panel[0].GetComponent<InventoryUI>();   
        stats = Panel[1].GetComponent<StatsUi>();   
        spellBook = Panel[2].GetComponent<SpellBookUI>();


        stats.Access();
        inventory.Access();
        spellBook.Access();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if(inventory)inventory.Cleaner();
        if(spellBook)spellBook.Cleaner();
    }

    private void OnDisable()
    {
        if (inventory) inventory.Cleaner();
        if(spellBook) spellBook.Cleaner();
    }
    public void Button(int index)
    {
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(i == index);
        }
    }
}
