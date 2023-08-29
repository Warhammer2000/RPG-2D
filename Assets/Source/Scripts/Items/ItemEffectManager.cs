using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemEffectManager
{
    [Inject] private SpellBook _spell;
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
        for(int i = 0; i < _spell.spells.Length; i++)
        {
            if(_spell.spells[i] != null)
            {
                if(_spell.spells[i].id == spell.id)
                {
                    Debug.Log("�� ��� ������, ��� ����������.");
                    return false;
                }
            }
        }
        for(int i = 0; i < _spell.spells.Length; i++)
        {
            if (_spell  .spells[i] == null)
            {
                _spell.spells[i] = spell;
                return true;
            }
        }
        Debug.Log("�� ������� ����� � ����� ����������");
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
