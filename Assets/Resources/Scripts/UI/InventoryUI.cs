using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public CellScript[] cells;
    private Transform cellPanel;
    public Transform cursor;
    private Image cursorImage;
    private Text cursorText;


    //Interactive
    //ItemManager itemManager
    //Inventory Settings
    public Item selectedItem = null;
    public int selectedCount = 0;
    public CellScript cursorCell;
    public CellScript selectedCell;
    public CellScript previousCell;
    
    [SerializeField]
    private Transform infoPanel;
    [SerializeField]
    private Image infoImage;
    [SerializeField]
    private Text infoName;
    [SerializeField]
    private Text infoDescription;
    [SerializeField]
    private Text infoEffect;
    [SerializeField]
    private Text infoCost;
    [SerializeField]
    //EquipPanel Settings

    private Transform equipPanel;
    private EquipCellScript[] equipCells;
    //Colors
    [Header("InventoryColors")]
    public Color cursorColor;
    public Color myColor;
    public Color sselectColor;
    public Color[] equipColor;
    private GameObject ItemPref;
    public void Access()
    {
        cellPanel = transform.GetChild(0);
        infoPanel = transform.GetChild(1);
        equipPanel = transform.GetChild(2);
        if (cursor)
        {
            cursorImage = cursor.GetComponent<Image>();
            cursorText = cursor.GetChild(0).GetComponent<Text>();
        }
        else Debug.Log("Вставить ссылку на курсор!");
        //Invetory
        cells = new CellScript[48];
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = cellPanel.GetChild(i).GetComponent<CellScript>().GetLinkSetSettings(i, this);
        }
        //Info//
        infoImage = infoPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        infoName = infoPanel.GetChild(1).GetComponent<Text>();  
        infoDescription = infoPanel.GetChild(2).GetComponent<Text>();  
        infoEffect = infoPanel.GetChild(3).GetComponent<Text>();

        equipCells = new EquipCellScript[3];
        for(int i = 0; i < equipCells.Length; i++)
        {
            equipCells[i] = equipPanel.GetChild(i).GetComponent<EquipCellScript>();
        }

        ItemPref = Resources.Load<GameObject>("Prefabs/Item");
    }

  
    void Update()
    {
        if (cursorCell)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectCellSwitch();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (Inventory.instance.Use(cursorCell.CellId)) RefreshAll();
                {
                    Debug.Log("Use");
                    SelectCellSwitch();
                    return;
                }
            }
        }
    }
    private void ClearCursor()
    {
        cursor.gameObject.SetActive(false);
        previousCell = null;
        selectedCell = cursorCell;
        RefreshAll();
    }

    private void InfoChange(Item ItemInfo)
    {
        if (ItemInfo)
        {
            infoImage.enabled = true;
            infoImage.sprite = ItemInfo.sprite;

            infoName.text = ItemInfo.name;
            infoDescription.text = ItemInfo.description;    
            infoEffect.text = ItemInfo.effect;
           
        }
        else
        {
            infoImage.enabled = false;
            infoName.text = "";
            infoDescription.text = "";
            infoEffect.text = "";
        }
    }
    public void RefreshAll()
    {
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i].isEquip = false;

            if(Inventory.instance.inv[cells[i].CellId] != null)
            {
                int index = (int)Inventory.instance.inv[i].myType;
                if(Inventory.instance.equipment.Length > index)
                {
                    if (Inventory.instance.equipment[index] == Inventory.instance.inv[i]) cells[i].isEquip = true;
                }
            }
            cells[i].Refresh();
         //   if (cells[i].isEquip) cells[i].SetColor(equipColor[(int)Inventory.instance.inv[cells[i].CellId].myType]);
          //  else cells[i].SetColor(myColor);
        }
        if (cursorCell && !cursorCell.isEquip) cursorCell.SetColor(cursorColor);
        if (selectedCell)
        {
            if (!selectedCell.isEquip) selectedCell.SetColor(sselectColor);

            InfoChange(Inventory.instance.inv[selectedCell.CellId]);
        }
        else InfoChange(null);

        for(int i = 0; i < equipCells.Length; i++)
        {
            equipCells[i].Refresh();
        }
    }
 
    public void Cleaner()
    {
        ClearCursor();
        cursorCell = null;
        selectedCell = null;
        RefreshAll();
    }
    private void SelectCellSwitch()
    {
        if (!selectedCell)
        {
            selectedCell = cursorCell;
        }
        else
        {
            if (selectedCell == cursorCell) selectedCell = null;
            else selectedCell = cursorCell;
        }
    }
    public void CursorCellSwitch(CellScript newCell)
    {
        if (!cursorCell)
        {
            cursorCell = newCell;
        }
        else cursorCell = null;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!cursorCell) return;
        if (!Inventory.instance.inv[cursorCell.CellId]) return;

        previousCell = cursorCell;
        cursorCell = previousCell;
        RefreshAll();
        cursor.gameObject.SetActive(true);
        cursorImage.sprite = Inventory.instance.inv[previousCell.CellId].sprite;
        cursorText.text = Inventory.instance.count[previousCell.CellId].ToString();
    }
    public void OnDrag(PointerEventData eventData)
    {
        cursor.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(cursorCell == previousCell)
        {
            ClearCursor();
            return;
        }
        if(cursorCell == null && previousCell)
        {
            DropItem();
            cursor.gameObject.SetActive(false);
            previousCell = null;
            ClearCursor();
            return ;
        }
        if(previousCell && cursorCell)
        {
            if (cursorCell.isFree) Inventory.instance.MoveItem(previousCell.CellId, cursorCell.CellId);
            else Inventory.instance.SwapItem(previousCell.CellId, cursorCell.CellId);
            ClearCursor();
        }
    }
    private void DropItem()
    {
        int index = (int)Inventory.instance.inv[previousCell.CellId].myType;
        if(Inventory.instance.equipment.Length > index)
        {
           if (Inventory.instance.equipment[index] == Inventory.instance.inv[previousCell.CellId])
           {
                Inventory.instance.equipment[index] = null;
                previousCell.isEquip = false;
           }
        }

        ItemSettings temp = Instantiate(ItemPref, PlayerController.con.transform.position, Quaternion.identity).GetComponent<ItemSettings>();
        temp.thisItem = Inventory.instance.inv[previousCell.CellId];
        temp.count = Inventory.instance.count[previousCell.CellId];

        Inventory.instance.inv[previousCell.CellId] = null;
        Inventory.instance.count[previousCell.CellId] = 0;
        RefreshAll();
    }
}
