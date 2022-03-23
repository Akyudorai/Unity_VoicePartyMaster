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
        Actions.Add("heal", new Action<int,string>(A1_Buffer));
        Actions.Add("barrier", new Action<int,string>(A2_Buffer));
        Actions.Add("resurrect", new Action<int,string>(A3_Buffer));
        Actions.Add("nova", new Action<int,string>(A4_Buffer));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Heal
    override public void A1(AbilitySettings settings) 
    {
        // Restores health to a target ally.

        // Set Cooldown

        Debug.Log("Heal Activated");
    }

    // Barrier
    override public void A2(AbilitySettings settings) 
    {
        // Give a target ally a shield buff that absorbs damage.

        // Set Cooldown

        Debug.Log("Barrier Activated");
    }

    // Resurrect
    override public void A3(AbilitySettings settings)
    {
        // Bring a fallen ally back to life

        // Set Cooldown

        Debug.Log("Resurrect Activated");
    }

    // Holy Nova
    override public void A4(AbilitySettings settings)
    {
        // Deal damage to all enemies and heal all allies in an area around the Priest

        // Set cooldown

        Debug.Log("Holy Nova Activated");
    }
}
