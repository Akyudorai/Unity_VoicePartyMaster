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
            Spell_Power = 4,
            Mana = 100.0f, 
            Mana_Regen = 1.0f,

            // Offensive Stats
            Critical_Rate = 0.1f,
            Critical_Damage = 2.0f,
        };

        // Ability Data
        Abilities.Add("A1", new AbilityData() {
            Name = "Heal",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A2", new AbilityData() {
            Name = "Barrier",
            CD = 6.0f,
            currCD = 0.0f
        });

        Abilities.Add("A3", new AbilityData() {
            Name = "Resurrect",
            CD = 60.0f,
            currCD = 0.0f
        });

        Abilities.Add("A4", new AbilityData() {
            Name = "Holy Nova",
            CD = 6.0f,
            currCD = 0.0f
        });

        // Character Commands
        Actions.Add("heal", new Action<int,string>(A1_Buffer));
        Actions.Add("barrier", new Action<int,string>(A2_Buffer));
        Actions.Add("resurrect", new Action<int,string>(A3_Buffer));
        Actions.Add("nova", new Action<int,string>(A4_Buffer));

        // SFX
        Sounds.Add("attack", Resources.Load<AudioClip>("Audio/Priest/Attack"));

        // UI
        Icons.Add("A1", Resources.Load<Sprite>("Icons/Priest/Heal"));
        Icons.Add("A2", Resources.Load<Sprite>("Icons/Priest/Barrier"));
        Icons.Add("A3", Resources.Load<Sprite>("Icons/Priest/Resurrect"));
        Icons.Add("A4", Resources.Load<Sprite>("Icons/Priest/Holy_Nova"));
    }

    // ============================================================
    // ABILITY FUNCTIONALITY OVERRIDES
    // ============================================================

    // Heal
    override public void A1(AbilitySettings settings) 
    {
        // Restores health to a target ally.
        float amount = GetStats().Spell_Power * 2.5f;             
        settings.target.DealDamage(-amount); 

        // Set Cooldown
        AbilityData A1 = Abilities["A1"];
        A1.currCD = A1.CD;
        Abilities["A1"] = A1;

        Debug.Log("Heal Activated");
    }

    // Barrier
    override public void A2(AbilitySettings settings) 
    {
        // Give a target ally a shield buff that absorbs damage.

        // Set Cooldown
        AbilityData A2 = Abilities["A2"];
        A2.currCD = A2.CD;
        Abilities["A2"] = A2;

        Debug.Log("Barrier Activated");
    }

    // Resurrect
    override public void A3(AbilitySettings settings)
    {
        // Bring a fallen ally back to life
        if (settings.target.IsDead) {
            settings.target.IsDead = false;
            settings.target.SetCurrentHealth(settings.target.GetMaxHealth() * (3/10));
            settings.target.GetAnimator().SetTrigger("Resurrect");
        }

        // Set Cooldown
        AbilityData A3 = Abilities["A3"];
        A3.currCD = A3.CD;
        Abilities["A3"] = A3;

        Debug.Log("Resurrect Activated");
    }

    // Holy Nova
    override public void A4(AbilitySettings settings)
    {
        // Create a Holy Nova
        GameObject nova = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Priest/VFX_HolyNova"), 
            settings.owner.GetOwner().transform.position,
            Quaternion.identity
            );

        nova.GetComponent<HolyNova>().Initialize(settings);

        // Set cooldown
        AbilityData A4 = Abilities["A4"];
        A4.currCD = A4.CD;
        Abilities["A4"] = A4;

        Debug.Log("Holy Nova Activated");
    }

    override protected void A4_Buffer(int order, string prevKeyword)
	{
		if (!isSelected)            
            return;        

        animator.SetTrigger("A4");
	}
    
}
