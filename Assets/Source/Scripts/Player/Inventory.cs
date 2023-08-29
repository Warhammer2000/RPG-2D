using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
  
    public Item[] inv; //items
    public int[] count;
    public int money;
    [SerializeField] private Text moneyText;
    public Item[] equipment;
    private ItemEffectManager itemEffectManager;
    public int arrowID;
    public Text MoneyDText;
    public GameObject UpgradeButton;
    private void Awake()
    {
        inv = new Item[42];
        count = new int[42];
        equipment = new Item[3];
        itemEffectManager = new ItemEffectManager();
        UpgradeButton.SetActive(false);
    }
    private void Start()
    {
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyText.text = "Money : " + money;
    }
    public bool AddItem(Item newItem, int newCount)
    {
        for(int i = 0; i < inv.Length; i++)
        {
            if(inv[i] && newItem.id == inv[i].id)
            {
                count[i] += newCount;
                if(count[i] >= 2 && inv[i].myType == Item.ItemTypes.melWeapon)
                {
                    UpgradeButton.SetActive(true); 
                }

                return true;
            }
        }
        for (int i = 0; i < inv.Length; i++)
        {
            if(inv[i] == null)
            {
                inv[i] = newItem;
                count[i] = newCount;
                return true;
            }
        }
        return false;
    }
    
    public void UpgradeItem(Item newItem)
    {
         for (int i = 0; i < inv.Length; i++)
        {
            if (inv[i].myType == Item.ItemTypes.melWeapon)
            {
                inv[i] = newItem;
                count[i]--;
            }
            else Debug.Log("not melWeapon");
        }

    }
    public bool Use(int id)
    {
        if (!inv[id]) return false;
        switch (inv[id].myType)
        {
            case Item.ItemTypes.item: return UseItem(id);  
            default:  SetEquip(inv[id].myType, id); return true; 
        }
  
    }
    private void SetEquip(Item.ItemTypes equipType, int id)
    {
        if (equipment[(int)equipType] == inv[id]) equipment[(int)equipType] = null;
        else equipment[(int)equipType] = inv[id];
    }
    public void AddGold(int count)
    {
        if (count >= 1)
        { 
            Debug.Log("Вы подняли " + count + "золота");
            money += count;
            moneyText.text = "Money : " + money;
            MoneyDText.text = "Money " + money; 
        }
        else Debug.Log("Вы подняли");
    }
    private bool UseItem(int id)
    {
        if(!inv[id].isUseFul) return false;
        if (itemEffectManager.GetEffect(inv[id]))
        {
            if(count[id] > 1)
            {
                count[id]--;
            }
            else
            {
                count[id] = 0;
                inv[id] = null;
            }
            return true;
        }
        else return false;
    }
    public void MoveItem(int oldid, int newId)
    {
        inv[newId] = inv[oldid];
        count[newId] = count[oldid];
        inv[oldid] = null;
        count[oldid] = 0;
    }
    public void SwapItem(int oldID, int newID)
    {
        Item tempItem = inv[newID];
        int tempCount = count[newID];

        inv[newID] = inv[oldID];
        count[newID] = count[oldID];

        inv[oldID] = tempItem;
        count[oldID] = tempCount;
    }
    public bool ArrowCheaker(int id)
    {
        for (int i = 0; i < inv.Length; i++)
        {
            if (inv[i])
            {
                if (inv[i].id == id)
                {
                    arrowID = i;
                    return true;
                }
            }
        }
        return false;
    }
    public void ArrowUse()
    {
        count[arrowID]--;
        InventoryRefresh();
    }
    private void InventoryRefresh()
    {
        for(int i = 0; i < inv.Length; i++)
        {
            if (count[i] <= 0) inv[i] = null;
        }
    }
}
