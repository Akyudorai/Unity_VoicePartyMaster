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
            Attack_Power = 5,
            Attack_Speed = 1.0f,

            // Magical Stats
            Spell_Power = 1,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Ability Data
        Abilities.Add("A1", new AbilityData() {
            Name = "Aimed Shot",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A2", new AbilityData() {
            Name = "Multishot",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A3", new AbilityData() {
            Name = "Rapid Fire",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A4", new AbilityData() {
            Name = "Disengage",
            CD = 12.0f,
            currCD = 0.0f
        });

        // Character Commands
        Actions.Add("aimed", new Action<int,string>(A1_Buffer));
        Actions.Add("aim", new Action<int,string>(A1_Buffer));
        Actions.Add("multishot", new Action<int,string>(A2_Buffer));
        Actions.Add("rapid", new Action<int,string>(A3_Buffer));
        Actions.Add("disengage", new Action<int,string>(A4_Buffer));

        // Sound Effects
        Sounds.Add("draw", Resources.Load<AudioClip>("Audio/Archer/Draw"));
        Sounds.Add("attack", Resources.Load<AudioClip>("Audio/Archer/Release"));

        // UI
        Icons.Add("A1", Resources.Load<Sprite>("Icons/Archer/Aimed_Shot"));
        Icons.Add("A2", Resources.Load<Sprite>("Icons/Archer/Multishot"));
        Icons.Add("A3", Resources.Load<Sprite>("Icons/Archer/Rapid_Fire"));
        Icons.Add("A4", Resources.Load<Sprite>("Icons/Archer/Disengage"));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Aimed Shot
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to the Target based on Attack Power
        float amount = GetStats().Attack_Power * 3.0f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A1 = Abilities["A1"];
        A1.currCD = A1.CD;
        Abilities["A1"] = A1;

        Debug.Log("Aimed Shot Activated");  
    }

    // Multi-Shot
    override public void A2(AbilitySettings settings) 
    {
        // Deal Damage to up to three Targets based on Attack Power
        float amount = GetStats().Attack_Power * 0.75f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A2 = Abilities["A2"];
        A2.currCD = A2.CD;
        Abilities["A2"] = A2;

        Debug.Log("Multi-Shot Activated");
    }

    // Rapid Fire
    override public void A3(AbilitySettings settings)
    {
        // Grant a buff that increases attack speed for X Seconds.

        // Set Cooldown
        AbilityData A3 = Abilities["A3"];
        A3.currCD = A3.CD;
        Abilities["A3"] = A3;

        Debug.Log("Rapid Fire Activated");
    }

    // Disengage
    override public void A4(AbilitySettings settings)
    {
        // Jump away from target enemy.

        // Set Cooldown
        AbilityData A4 = Abilities["A4"];
        A4.currCD = A4.CD;
        Abilities["A4"] = A4;


        Debug.Log("Disengage Activated");
    }
}
