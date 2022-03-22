using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Actions.Add("Fireball", Fireball);
        Actions.Add("Arcane Nova", ArcaneNova);
        Actions.Add("Blink", Blink);
        Actions.Add("Blizzard", Blizzard);
    }

    // Fireball, shoot a fireball at a target
    private void Fireball() {
        if (!isSelected)
            return;
            
        Debug.Log("Fireball Activated");
    }

    // Arcane Nova, deal damage in an area around the Mage
    private void ArcaneNova() {
        if (!isSelected)
            return;
            
        Debug.Log("Arcane Nova Activated");
    }

    // Blink, teleport a short distance in a target direction
    private void Blink() {
        if (!isSelected)
            return;
            
        Debug.Log("Blink Activated");
    }

    // Blizzard, channel an AoE damage over time spell at a target location
    private void Blizzard() {
        if (!isSelected)
            return;
            
        Debug.Log("Blizzard Activated");
    }
}
