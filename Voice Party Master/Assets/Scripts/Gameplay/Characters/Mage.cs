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
        Actions.Add("dragonbreath", new Action<int,string>(DragonsBreath));
        Actions.Add("dragons", new Action<int,string>(DragonsBreath));
        Actions.Add("burst", new Action<int,string>(FlameBurst));
        Actions.Add("blast", new Action<int,string>(FireBlast));
        Actions.Add("meteor", new Action<int,string>(Meteor));
    }

    // Fireball, shoot a fireball at a target
    private void DragonsBreath(int order, string prevKeyword) {
        if (!isSelected)
            return;
        
        animator.SetTrigger("A1");
        Debug.Log("Dragons Breath Activated");
    }

    // Arcane Nova, deal damage in an area around the Mage
    private void FlameBurst(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A2");
        Debug.Log("Flame Burst Activated");
    }

    // Blink, teleport a short distance in a target direction
    private void FireBlast(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A3");
        Debug.Log("Fire Blast Activated");
    }

    // Blizzard, channel an AoE damage over time spell at a target location
    private void Meteor(int order, string prevKeyword) {
        if (!isSelected)
            return;
            
        animator.SetTrigger("A4");
        Debug.Log("Meteor Activated");
    }
}
