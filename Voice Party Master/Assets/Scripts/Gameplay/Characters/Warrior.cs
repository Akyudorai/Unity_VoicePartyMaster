using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Warrior : Character
{
    public Warrior() {
        characterName = "Warrior";
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
        Actions.Add("slam", new Action<int,string>(A1_Buffer));
        Actions.Add("shout", new Action<int,string>(A2_Buffer));
        Actions.Add("taunt", new Action<int,string>(A2_Buffer));
        Actions.Add("charge", new Action<int,string>(A3_Buffer));
        Actions.Add("wall", new Action<int,string>(A4_Buffer));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Shield Slam
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to the Target based on Attack Power and Defense
        float amount = GetStats().Attack_Power * 0.75f + GetStats().Armor * 0.5f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown

        Debug.Log("Shield Slam Activated");       
    }

    // Mocking Shout
    override public void A2(AbilitySettings settings) 
    {
        // Force a specified target to attack the Warrior.

        // Set Cooldown

        Debug.Log("Mocking Shout Activated");
    }

    // Charge
    override public void A3(AbilitySettings settings)
    {
        // Target, Engage, and Sprint at an increased speed.

        // Set Cooldown

        Debug.Log("Charge Activated");
    }

    // Shield Wall
    override public void A4(AbilitySettings settings)
    {
        // Grant Defense Buff for X seconds

        // Set Cooldown

        Debug.Log("Shield Wall Activated");
    }
}
