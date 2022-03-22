using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Actions.Add("Aimed Shot", AimedShot);
        Actions.Add("Multi Shot", MultiShot);
        Actions.Add("Rapid Fire", RapidFire);
        Actions.Add("Disengage", Disengage);
    }

    // Aimed Shot, charge up a powerful shot
    private void AimedShot() {
        if (!isSelected)
            return;
            
        Debug.Log("Aimed Shot Activated");
    }

    // Multi-shot, shoot multiple arrows in a cone 
    private void MultiShot() {
        if (!isSelected)
            return;
            
        Debug.Log("Multi-Shot Activated");
    }

    // Rapid Fire, increase attack speed for a period of time
    private void RapidFire() {
        if (!isSelected)
            return;
            
        Debug.Log("Rapid Fire Activated");
    }

    // Disengage, move quickly in a target direction and reduce threat
    private void Disengage() {
        if (!isSelected)
            return;
            
        Debug.Log("Disengage Activated");
    }

}
