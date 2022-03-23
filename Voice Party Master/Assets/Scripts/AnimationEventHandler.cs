using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private PlayerController pc;
    private Animator animator;

    public bool DebugMe = false;

    private void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (DebugMe) Debug.Log("Attack() called");

        // Check if target exists
        if (pc.target != null) {            
        
            // Deal Damage to Target
            float amount = (pc.GetCharacter().GetDamageType() == PrimaryDamageType.Physical) 
                ? pc.GetCharacter().GetStats().Attack_Power * 1.0f : pc.GetCharacter().GetStats().Spell_Power * 0.35f;                
            pc.target.GetComponent<PlayerController>().entity.DealDamage(amount); // Change to Entity or Enemy Class rather han PC.entity

            // Alternate Attack Animation
            if (pc.characterClass == CharacterClass.Rogue) {
                animator.SetBool("Alternate Attack", !animator.GetBool("Alternate Attack"));
            }
        }

        
    }

    public void AbilityOne()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        settings.target = pc.target.GetComponent<PlayerController>().entity; // Change to entity or Enemy Class Rather than PC.entity
        
        pc.GetCharacter().A1(settings);
    }

    public void AbilityTwo()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        settings.target = pc.target.GetComponent<PlayerController>().entity; // Change to entity or Enemy Class Rather than PC.entity
        
        pc.GetCharacter().A2(settings);
    }

    public void AbilityThree()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        settings.target = pc.target.GetComponent<PlayerController>().entity; // Change to entity or Enemy Class Rather than PC.entity
        
        pc.GetCharacter().A3(settings);
    }

    public void AbilityFour()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        settings.target = pc.target.GetComponent<PlayerController>().entity; // Change to entity or Enemy Class Rather than PC.entity
        
        pc.GetCharacter().A4(settings);
    }
}
