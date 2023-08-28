using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipCellScript : MonoBehaviour
{
    [Inject]
    private Inventory inventory;
    public enum WhatEquip
    {
        melle = 0, distant = 1, armor = 2
    }
    public WhatEquip equip = WhatEquip.melle;

    public void Refresh()
    {
        if(inventory.equipment[(int)equip] != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Image>().sprite = inventory.equipment[(int)equip].sprite;
        }
        else
        {
            transform.GetChild (0).gameObject.SetActive(false);
        }
    }
}
