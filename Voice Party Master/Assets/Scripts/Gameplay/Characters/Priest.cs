using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Priest : Character
{
    public Priest() {
        characterName = "Priest";
        damageType = PrimaryDamageType.Magical;
        base_stats = new CharacterStats() 
        {
            // Base Stats
            Movement_Speed = 1.0f,
            Attack_Range = 10.0f,
            
            // Defensive Stats
            Health = 100.0f, 
            Health_Regen = 1.0f,
            Armor = 1,
            Resistance = 1,

            // Physical Stats
            Attack_Power = 1,
            Attack_Speed = 1.0f,

            // Magical Stats
            Spell_Power = 100,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Character Commands
        Actions.Add("heal", new Action<int,string>(Heal));
        Actions.Add("barrier", new Action<int,string>(Barrier));
        Actions.Add("resurrect", new Action<int,string>(Resurrect));
        Actions.Add("nova", new Action<int,string>(HolyNova));
    }

    // Heal, a lesser healing ability that can be used frequently
    private void Heal(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A1");

        Debug.Log("Heal Activated");
    }

    // Barrier, create a protective barrier around a target
    private void Barrier(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A2");

        Debug.Log("Barrier Activated");
    }

    // Ressurect, bring a fallen ally back to life
    private void Resurrect(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A3");

        Debug.Log("Resurrect Activated");
    }

    // Holy Nova, an AoE heal around the Priest
    private void HolyNova(int order, string prevKeyword) {
        if (!isSelected)
            return;

        animator.SetTrigger("A4");
        Debug.Log("Holy Nova Activated");
    }
}
