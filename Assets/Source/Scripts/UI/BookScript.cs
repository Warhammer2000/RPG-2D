using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int BookID = 0;
    private SpellBookUI ui;

    public bool isSelect = false;
    public bool isFree = true;
    public bool isEquip = false;
    private Image myImage;

    private Image myIcon;
    private Text myCount;
  
    public void Refresh()
    {
        if (SpellBook.instance.spells[BookID])
        {
            isFree = false;
            myIcon.gameObject.SetActive(true);
            myIcon.sprite = SpellBook.instance.spells[BookID].spellIcon;
        }
        else
        {
            isFree=true;
            myIcon.gameObject.SetActive(false); 
        }
    }
    public void SetColor(Color newColor)
    {
        myImage.color = newColor;
    }
    public BookScript GetLinkSetSettings(int newID, SpellBookUI newUI)
    {
        BookID = newID;
        ui = newUI;
        isFree = true;
        myImage = GetComponent<Image>();
        myIcon = transform.GetChild(0).GetComponent<Image>();
        return this;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui) ui.CursorBookSwitch(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui) ui.CursorBookSwitch(this);
    }
}
