using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public new string name = "Предмет";
    public Sprite sprite;
    public Sprite spriteForBow_2;
    public GameObject myArrow;
    public AudioClip clip;

    [TextArea]
    public string description = "Здесь должно быть атмосферное описание предмета";
    [TextArea]
    public string effect = "Здесь должно быть описание свойства предмета";
    public int cost;

    public enum ItemTypes
    {
        melWeapon =0, disWeapon = 1, armor = 2, item = 3, gold = 4, arrow = 5, spell = 6, 
    }
    public ItemTypes myType = ItemTypes.item;

    [Header("Item Settings")]
    public bool isUseFul;
    public bool isArrow;
    public int ArrowId;
    public Sprite arrowSprite;
    [Header("Weapon Settings")]
    public int damage;
    public float pulse;
    public float speed;
    public float weight;
    [Space]
    public float length;
    public float offset;
    [Header("Armor Settings")]
    public int protection;
    [Header("Book Settings")]
    public bool bookSpell;
    public Spell BookSpell;
}
