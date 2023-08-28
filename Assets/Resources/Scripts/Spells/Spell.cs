using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public int id;
    public Sprite spellIcon;
    public string spellName = "����������";
    [TextArea]
    public string spellDescription = "����� ������ ���� ����������� �������� ����������";
    [TextArea]
    public string spellEffect = "����� ������ ���� �������� �������� ����������";

    public Color SpellColor;
    public GameObject spellPref;

    public enum SpellType
    {
        isAttackSpell = 0, isEffect = 1
    }
    public SpellType myType = SpellType.isAttackSpell;
    public int manaCost;

    [Header("Attack Spell Settings")]
    public int damage;
    public float pulse;
    public float speed;
    public bool isPiercing;
    public float explosionRadius;
    [Header("Effect Spell Settings")]
    public float time;
}
