using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemEffectManager
{
    public bool GetEffect(Item item)
    {
        bool _return = false;
        switch (item.id)
        {
            case 0: _return = Apple(); break;
            case 200: _return = LearnSpell(item.BookSpell); break;
            default: _return = false; break;
        }
        return _return; 
    }
    private bool LearnSpell(Spell spell)
    {
        for(int i = 0; i < SpellBook.instance.spells.Length; i++)
        {
            if (SpellBook.instance.spells[i] != null)
            {
                if (SpellBook.instance.spells[i].id == spell.id)
                {
                    Debug.Log("Вы уже знаете, это заклинание.");
                    return false;
                }
            }
        }
        for(int i = 0; i < SpellBook.instance.spells.Length; i++)
        {
            if (SpellBook.instance  .spells[i] == null)
            {
                SpellBook.instance.spells[i] = spell;
                return true;
            }
        }
        Debug.Log("Не хватает места в книге заклинаний");
        return false;
    }
    private bool Apple()
    {
        if(PlayerStats.PlayerHealth < PlayerStats.PlayerMaxHealth)
        {
            PlayerStats.PlayerHealth += 30;
            Debug.Log("apple");
            return true;
        }
        else return false;
    }
}
