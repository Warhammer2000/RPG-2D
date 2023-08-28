using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuingScript : MonoBehaviour
{
    [Inject]
    private Inventory inventroy;
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
        if(inventroy.money >= itemforBuy.cost)
        {
           AddItem(itemforBuy, count);
           inventroy.money -= itemforBuy.cost;    
        }
        else
        {
            untillBuy.text = "Вам нехватает " + (itemforBuy.cost - inventroy.money) + " золота, для покупки " + itemforBuy.name;
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
        for (int i = 0; i < inventroy.inv.Length; i++)
        {
            if (inventroy.inv[i] && newItem.id == inventroy.inv[i].id)
            {
                inventroy.count[i] += newCount;
                return true;
            }
        }
        for (int i = 0; i < inventroy.inv.Length; i++)
        {
            if (inventroy.inv[i] == null)
            {
                inventroy.inv[i] = newItem;
                inventroy.count[i] = newCount;
                return true;
            }
        }
        return false;
    }
}
