using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mage : Character
{
    public Mage() {
        characterName = "Mage";
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
        Actions.Add("dragonbreath", new Action<int,string>(A1_Buffer));
        Actions.Add("dragons", new Action<int,string>(A1_Buffer));
        Actions.Add("burst", new Action<int,string>(A2_Buffer));
        Actions.Add("blast", new Action<int,string>(A3_Buffer));
        Actions.Add("meteor", new Action<int,string>(A4_Buffer));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Dragons Breath
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to all targets in a line X number of times over Y seconds

        // Set Cooldown

        Debug.Log("Dragons Flame Activated");
    }

    // Flame Burst
    override public void A2(AbilitySettings settings) 
    {
        // Deal Damage to all targets in an area around the Mage

        // Set Cooldown

        Debug.Log("Flame Burst Activated");
    }

    // Fire Blast
    override public void A3(AbilitySettings settings)
    {
        // Deal Damage to a target dealing damage based on spell power
        float amount = GetStats().Spell_Power * 2.5f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown

        Debug.Log("Fire Blast Activated");
    }

    // Meteor
    override public void A4(AbilitySettings settings)
    {
        // Drop a meteor at a target location that deals damage to all targets within a radius.

        // Set Cooldown

        Debug.Log("Meteor Activated");
    }
}
