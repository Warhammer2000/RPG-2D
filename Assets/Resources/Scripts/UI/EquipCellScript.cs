using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCellScript : MonoBehaviour
{
    public enum WhatEquip
    {
        melle = 0, distant = 1, armor = 2
    }
    public WhatEquip equip = WhatEquip.melle;

    public void Refresh()
    {
        if(Inventory.instance.equipment[(int)equip] != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Image>().sprite = Inventory.instance.equipment[(int)equip].sprite;
        }
        else
        {
            transform.GetChild (0).gameObject.SetActive(false);
        }
    }
}
