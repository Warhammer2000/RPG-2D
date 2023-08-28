using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public Spell[] spells;
    public Spell[] equipment;
    public static SpellBook instance;

    private void Awake()
    {
        instance = this;
        spells = new Spell[18];
        equipment = new Spell[2];
    }
    public void SetEquip(int eq, int id)
    {
        if (equipment[eq] == spells[id]) equipment[eq] = null;
        else equipment[eq] = spells[id];
    }
}
