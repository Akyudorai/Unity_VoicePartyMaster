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
            Attack_Range = 4.0f,
            
            // Defensive Stats
            Health = 100.0f, 
            Health_Regen = 1.0f,
            Armor = 1,
            Resistance = 1,

            // Physical Stats
            Attack_Power = 7,
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
            Name = "Shield Slam",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A2", new AbilityData() {
            Name = "Mocking Shout",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A3", new AbilityData() {
            Name = "Charge",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A4", new AbilityData() {
            Name = "Shield Wall",
            CD = 12.0f,
            currCD = 0.0f
        });

        // Character Commands
        Actions.Add("slam", new Action<int,string>(A1_Buffer));
        Actions.Add("shout", new Action<int,string>(A2_Buffer));
        Actions.Add("taunt", new Action<int,string>(A2_Buffer));
        Actions.Add("charge", new Action<int,string>(A3_Buffer));
        Actions.Add("wall", new Action<int,string>(A4_Buffer));

        // SFX
        Sounds.Add("attack", Resources.Load<AudioClip>("Audio/Warrior/Attack"));

        // UI
        Icons.Add("A1", Resources.Load<Sprite>("Icons/Warrior/Shield_Slam"));
        Icons.Add("A2", Resources.Load<Sprite>("Icons/Warrior/Mocking_Shout"));
        Icons.Add("A3", Resources.Load<Sprite>("Icons/Warrior/Charge"));
        Icons.Add("A4", Resources.Load<Sprite>("Icons/Warrior/Shield_Wall"));

    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Shield Slam
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to the Target based on Attack Power and Defense
        float amount = GetStats().Attack_Power * 2 + GetStats().Armor * 1.5f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A1 = Abilities["A1"];
        A1.currCD = A1.CD;
        Abilities["A1"] = A1;

        Debug.Log("Shield Slam Activated");       
    }

    // Mocking Shout
    override public void A2(AbilitySettings settings) 
    {
        // Force a specified target to attack the Warrior.

        // Set Cooldown
        AbilityData A2 = Abilities["A2"];
        A2.currCD = A2.CD;
        Abilities["A2"] = A2;

        Debug.Log("Mocking Shout Activated");
    }

    // Charge
    override public void A3(AbilitySettings settings)
    {
        // Target, Engage, and Sprint at an increased speed.

        // Set Cooldown
        AbilityData A3 = Abilities["A3"];
        A3.currCD = A3.CD;
        Abilities["A3"] = A3;

        Debug.Log("Charge Activated");
    }

    // Shield Wall
    override public void A4(AbilitySettings settings)
    {
        // Grant Defense Buff for X seconds

        // Set Cooldown
        AbilityData A4 = Abilities["A4"];
        A4.currCD = A4.CD;
        Abilities["A4"] = A4;        

        Debug.Log("Shield Wall Activated");
    }
}
