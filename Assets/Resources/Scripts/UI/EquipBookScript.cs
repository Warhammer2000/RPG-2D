using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBookScript : MonoBehaviour
{
    public enum equipType
    {
        firstHand = 0,
        secondHand = 1,
    }
    public equipType myType = equipType.firstHand;
    public void Refresh()
    {
        if (SpellBook.instance.equipment[(int)myType]!= null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Image>().sprite = SpellBook.instance.equipment[(int)myType].spellIcon;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
