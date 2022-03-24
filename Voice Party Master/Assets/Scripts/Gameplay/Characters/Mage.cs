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
            Spell_Power = 7,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Ability Data
        Abilities.Add("A1", new AbilityData() {
            Name = "Dragons Breath",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A2", new AbilityData() {
            Name = "Flame Burst",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A3", new AbilityData() {
            Name = "Fireblast",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A4", new AbilityData() {
            Name = "Meteor",
            CD = 12.0f,
            currCD = 0.0f
        });

        // Character Commands
        Actions.Add("dragonbreath", new Action<int,string>(A1_Buffer));
        Actions.Add("dragons", new Action<int,string>(A1_Buffer));
        Actions.Add("burst", new Action<int,string>(A2_Buffer));
        Actions.Add("blast", new Action<int,string>(A3_Buffer));
        Actions.Add("meteor", new Action<int,string>(A4_Buffer));

        // SFX
        Sounds.Add("attack", Resources.Load<AudioClip>("Audio/Mage/Attack"));

        // UI
        Icons.Add("A1", Resources.Load<Sprite>("Icons/Mage/Dragons_Breath"));
        Icons.Add("A2", Resources.Load<Sprite>("Icons/Mage/Flame_Burst"));
        Icons.Add("A3", Resources.Load<Sprite>("Icons/Mage/Fireblast"));
        Icons.Add("A4", Resources.Load<Sprite>("Icons/Mage/Meteor"));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Dragons Breath
    override public void A1(AbilitySettings settings) 
    {
        // Deal Damage to all targets in a line X number of times over Y seconds
        float amount = GetStats().Spell_Power * 0.75f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A1 = Abilities["A1"];
        A1.currCD = A1.CD;
        Abilities["A1"] = A1;

        Debug.Log("Dragons Flame Activated");
    }

    // Flame Burst
    override protected void A2_Buffer(int order, string prevKeyword) 
    {   
        if (!isSelected)            
            return;  

        animator.SetTrigger("A2");
    }
     
    override public void A2(AbilitySettings settings) 
    {       
        // Create a Flame Burst
        GameObject burst = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Mage/VFX_FlameBurst"), 
            settings.owner.GetOwner().transform.position,
            Quaternion.identity
            );
        
        burst.GetComponent<FlameBurst>().Initialize(settings);
 
        // Set Cooldown
        AbilityData A2 = Abilities["A2"];
        A2.currCD = A2.CD;
        Abilities["A2"] = A2;

        Debug.Log("Flame Burst Activated");
    }

    // Fire Blast
    override public void A3(AbilitySettings settings)
    {
        // Deal Damage to a target dealing damage based on spell power
        float amount = GetStats().Spell_Power * 2.5f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A3 = Abilities["A3"];
        A3.currCD = A3.CD;
        Abilities["A3"] = A3;

        Debug.Log("Fire Blast Activated");
    }

    // Meteor
    override public void A4(AbilitySettings settings)
    {
        // Drop a meteor at a target location that deals damage to all targets within a radius.
        float amount = GetStats().Spell_Power * 2.5f;             
        settings.target.DealDamage(amount); 

        // Set Cooldown
        AbilityData A4 = Abilities["A4"];
        A4.currCD = A4.CD;
        Abilities["A4"] = A4;

        Debug.Log("Meteor Activated");
    }
}
