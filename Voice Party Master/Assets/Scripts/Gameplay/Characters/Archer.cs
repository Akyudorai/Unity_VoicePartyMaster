using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Archer : Character
{
    public Archer() {
        characterName = "Archer";
        damageType = PrimaryDamageType.Physical;
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
        Actions.Add("aimed", new Action<int,string>(A1_Buffer));
        Actions.Add("aim", new Action<int,string>(A1_Buffer));
        Actions.Add("multishot", new Action<int,string>(A2_Buffer));
        Actions.Add("rapid", new Action<int,string>(A3_Buffer));
        Actions.Add("disengage", new Action<int,string>(A4_Buffer));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Aimed Shot
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to the Target based on Attack Power
        float amount = GetStats().Attack_Power * 2.0f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown

        Debug.Log("Aimed Shot Activated");  
    }

    // Multi-Shot
    override public void A2(AbilitySettings settings) 
    {
        // Deal Damage to up to three Targets based on Attack Power
        float amount = GetStats().Attack_Power * 0.575f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown

        Debug.Log("Multi-Shot Activated");
    }

    // Rapid Fire
    override public void A3(AbilitySettings settings)
    {
        // Grant a buff that increases attack speed for X Seconds.

        // Set Cooldown

        Debug.Log("Rapid Fire Activated");
    }

    // Disengage
    override public void A4(AbilitySettings settings)
    {
        // Jump away from target enemy.

        // Set Cooldown

        
        Debug.Log("Disengage Activated");
    }
}
