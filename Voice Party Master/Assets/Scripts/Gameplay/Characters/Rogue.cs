using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Rogue : Character
{
    public Rogue() {
        characterName = "Rogue";
        damageType = PrimaryDamageType.Physical;
        base_stats = new CharacterStats() 
        {
            // Base Stats
            Movement_Speed = 1.0f,
            Attack_Range = 2.5f,
            
            // Defensive Stats
            Health = 100.0f, 
            Health_Regen = 1.0f,
            Armor = 1,
            Resistance = 1,

            // Physical Stats
            Attack_Power = 1,
            Attack_Speed = 1.0f,

            // Magical Stats
            Spell_Power = 1,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Character Commands
        Actions.Add("double", new Action<int,string>(DoubleStrike));
        Actions.Add("mutilate", new Action<int,string>(Mutilate));
        Actions.Add("stealth", new Action<int,string>(Stealth));
        Actions.Add("ambush", new Action<int,string>(Ambush));
    }

    // Double Strike, strike twice and gain a combo point
    private void DoubleStrike(int order, string prevKeyword) {
        if (!isSelected)
            return;

        animator.SetTrigger("A1");

        Debug.Log("Double Strike Activated");
    }

    // Eviscerate, spend combo points to deal large damage
    private void Mutilate(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A2");

        Debug.Log("Mutilate Activated");
    }

    // Stealth, go invisible to enemies and slow speed
    private void Stealth(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A3");

        Debug.Log("Stealth Activated");
    }

    // Ambush, only usable from stealth, deal massive damage
    private void Ambush(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A4");

        Debug.Log("Ambush Activated");
    }
}
