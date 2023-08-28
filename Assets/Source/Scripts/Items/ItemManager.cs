using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] item;
    public static Item[] items { get; private set; }
    private void Awake()
    {
        items = item;
    }
}
