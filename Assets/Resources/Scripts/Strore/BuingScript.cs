using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuingScript : MonoBehaviour
{

    public Item itemforBuy;
    public int count;
    public Image ItemImage;
    public Text itemText;
    public GameObject WarningPanel;
    public Text untillBuy;
    private void Awake()
    {
        itemText = transform.GetChild(0).GetComponent<Text>();
        ItemImage = transform.GetChild(1).GetComponent<Image>();

        itemText.text = " Gold : " + itemforBuy.cost.ToString();
        ItemImage.sprite = itemforBuy.sprite;
        WarningPanel.SetActive(false);
    }
    public void Buy()
    {
        if(Inventory.instance.money >= itemforBuy.cost)
        {
           AddItem(itemforBuy, count);
           Inventory.instance.money -= itemforBuy.cost;    
        }
        else
        {
            untillBuy.text = "Вам нехватает " + (itemforBuy.cost - Inventory.instance.money) + " золота, для покупки " + itemforBuy.name;
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        WarningPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        WarningPanel.SetActive(false);
    }
    public bool AddItem(Item newItem, int newCount)
    {
        for (int i = 0; i < Inventory.instance.inv.Length; i++)
        {
            if (Inventory.instance.inv[i] && newItem.id == Inventory.instance.inv[i].id)
            {
                Inventory.instance.count[i] += newCount;
                return true;
            }
        }
        for (int i = 0; i < Inventory.instance.inv.Length; i++)
        {
            if (Inventory.instance.inv[i] == null)
            {
                Inventory.instance.inv[i] = newItem;
                Inventory.instance.count[i] = newCount;
                return true;
            }
        }
        return false;
    }
}
