using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Actions.Add("Double Strike", DoubleStrike);
        Actions.Add("Eviscerate", Eviscerate);
        Actions.Add("Stealth", Stealth);
        Actions.Add("Ambush", Ambush);
    }

    // Double Strike, strike twice and gain a combo point
    private void DoubleStrike() {
        if (!isSelected)
            return;

        Debug.Log("Double Strike Activated");
    }

    // Eviscerate, spend combo points to deal large damage
    private void Eviscerate() {
        if (!isSelected)
            return;
            
        Debug.Log("Eviscerate Activated");
    }

    // Stealth, go invisible to enemies and slow speed
    private void Stealth() {
        if (!isSelected)
            return;
            
        Debug.Log("Stealth Activated");
    }

    // Ambush, only usable from stealth, deal massive damage
    private void Ambush() {
        if (!isSelected)
            return;
            
        Debug.Log("Ambush Activated");
    }
}
