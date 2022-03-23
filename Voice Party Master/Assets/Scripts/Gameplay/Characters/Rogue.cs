using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        Actions.Add("double", new Action<int,string>(A1_Buffer));
        Actions.Add("mutilate", new Action<int,string>(A2_Buffer));
        Actions.Add("stealth", new Action<int,string>(A3_Buffer));
        Actions.Add("ambush", new Action<int,string>(A4_Buffer));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Double Strike
    // ** GETS CALLED TWICE PER ABILITY CAST ** //
    override public void A1(AbilitySettings settings) 
    {        
        // Deal Damage to the Target based on Attack Power
        float amount = GetStats().Attack_Power * 0.575f;             
        settings.target.DealDamage(amount); 

        // Add a combo point
        if (animator.GetInteger("Combo Points") < 5) {
            animator.SetInteger("Combo Points", animator.GetInteger("Combo Points") + 1);
        }

        Debug.Log("Double Strike Activated");       
    }

    // Mutilate
    override public void A2(AbilitySettings settings) 
    {
        // Deal damage to target based on attack power and number of combo points
        float amount = GetStats().Attack_Power * 0.75f;
        amount *= animator.GetInteger("Combo Points");                     
        settings.target.DealDamage(amount); 

        // Reset Combo Points
        animator.SetInteger("Combo Points", 0);

        Debug.Log("Mutilate Activated");
    }

    // Stealth
    override public void A3(AbilitySettings settings)
    {
        // Change Material/Shader to indicate stealthed.

        // Switch Animation State
        if (animator.GetBool("Stealthed")) {
            animator.SetTrigger("Unstealth");
            animator.SetBool("Stealthed", false);
        } else {
            animator.SetTrigger("A3");
            animator.SetBool("Stealthed", true);
        }

        Debug.Log("Stealth " + (animator.GetBool("Stealthed") ? "Activated" : "Deactivated"));
    }

    // Ambush
    override public void A4(AbilitySettings settings)
    {
        // Deal Damage to the Target based on Attack Power
        float amount = GetStats().Attack_Power * 2.25f;             
        settings.target.DealDamage(amount); 

        // Exit Stealth
        animator.SetTrigger("Unstealth");
        animator.SetBool("Stealthed", false);

        Debug.Log("Ambush Activated");
    }

    // ============================================================
    // ABILITY BUFFER OVERRIDES
    // ============================================================

    override protected void A2_Buffer(int order, string prevKeyword) 
    {
        // Must have at least one combo point to use the ability.
        if (animator.GetInteger("Combo Points") == 0) {
            Debug.Log("I need combo points to use that ability.");
            return;
        }
    }
    
    override protected void A4_Buffer(int order, string prevKeyword) 
    {   
        // Must be stealthed to use the ability.
        if (!animator.GetBool("Stealthed")) {
            Debug.Log("I must be stealthed to use that ability.");
            return;
        }
    }
}
