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
        Actions.Add("slam", new Action<int,string>(ShieldSlam));
        Actions.Add("shout", new Action<int,string>(Taunt));
        Actions.Add("taunt", new Action<int,string>(Taunt));
        Actions.Add("charge", new Action<int,string>(Charge));
        Actions.Add("wall", new Action<int,string>(ShieldWall));
    }

    // Shield Slam, deal damage to target and stun them briefly
    private void ShieldSlam(int order, string prevKeyword) {
                
        if (!isSelected)            
            return;            

        animator.SetTrigger("A1");

        Debug.Log("Shield Slam Activated");
    }

    // Taunt, force a target to attack the Warrior
    private void Taunt(int order, string prevKeyword) {
        if (!isSelected)
            return;

        animator.SetTrigger("A2");

        Debug.Log("Taunt Activated");
    }

    // Charge, charge at a target forcing it to target you 
    private void Charge(int order, string prevKeyword) {
                
        if (!isSelected)
            return;

        animator.SetTrigger("A3");

        Debug.Log("Charge Activated");
    }

    // Shield Wall, take reduced damage for a period of time.
    private void ShieldWall(int order, string prevKeyword) {
        if (!isSelected)
            return;
        
        animator.SetTrigger("A4");

        Debug.Log("Shield Wall Activated");
    }
}
