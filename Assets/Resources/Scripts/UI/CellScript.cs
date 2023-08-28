using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int CellId;
    private InventoryUI ui;

    public bool isSelect = false;
    public bool isFree = true;
    public bool isEquip = false;
    private Image myImage;

    private Image myIcon;
    private Text myCount;
    
    public void Refresh()
    {
        if (Inventory.instance.inv[CellId])
        {
            isFree = false;
            myIcon.gameObject.SetActive(true);
            myCount.gameObject.SetActive(true);
            myIcon.sprite = Inventory.instance.inv[CellId].sprite;
            myCount.text = Inventory.instance.count[CellId].ToString(); 
        }
        else
        {
            isFree = true;  
            myIcon.gameObject.SetActive(false);
            myCount.gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui) ui.CursorCellSwitch(this);
        ui.cursorCell = GetComponent<CellScript>();
        if (!isSelect && !isEquip)
        {
            transform.GetComponent<Image>().color = ui.sselectColor;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui) ui.CursorCellSwitch(this);
        ui.cursorCell = null;
        if (!isSelect && !isEquip)
        {
            transform.GetComponent<Image>().color = ui.myColor;
        }
    }
    public CellScript GetLinkSetSettings(int newId, InventoryUI newUi)
    {
        CellId = newId;
        ui = newUi;
        isFree = true;
        myImage = GetComponent<Image>();
        myIcon = transform.GetChild(0).GetComponent<Image>();
        myCount = transform.GetChild(1).GetComponent<Text>();
        return this;
    }
    public void SetColor(Color newColor)
    {
        myImage.color = newColor;
    }
}
