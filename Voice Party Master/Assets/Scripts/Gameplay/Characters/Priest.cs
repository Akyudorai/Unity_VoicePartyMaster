using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Spell_Power = 1,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Character Commands
        Actions.Add("Heal", Heal);
        Actions.Add("Barrier", Barrier);
        Actions.Add("Ressurect", Ressurect);
        Actions.Add("Holy Nova", HolyNova);
    }

    // Heal, a lesser healing ability that can be used frequently
    private void Heal() {
        if (!isSelected)
            return;
            
        Debug.Log("Heal Activated");
    }

    // Barrier, create a protective barrier around a target
    private void Barrier() {
        if (!isSelected)
            return;
            
        Debug.Log("Barrier Activated");
    }

    // Ressurect, bring a fallen ally back to life
    private void Ressurect() {
        if (!isSelected)
            return;
            
        Debug.Log("Ressurect Activated");
    }

    // Holy Nova, an AoE heal around the Priest
    private void HolyNova() {
        if (!isSelected)
            return;
            
        Debug.Log("Holy Nova Activated");
    }
}
