using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationEventHandler : MonoBehaviour
{
    private PlayerController pc;
    private Animator animator;
    private AudioSource audioSource;


    public bool DebugMe = false;

    private void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(string str) 
    {
        //Debug.Log(str);

        audioSource.PlayOneShot(Resources.Load<AudioClip>(str));
    }

    public void Attack()
    {
        if (DebugMe) Debug.Log("Attack() called");

        // Check if target exists
        if (pc.target != null) {            
        
            // Deal Damage to Target
            float amount = (pc.GetCharacter().GetDamageType() == PrimaryDamageType.Physical) 
                ? pc.GetCharacter().GetStats().Attack_Power * 1.0f : pc.GetCharacter().GetStats().Spell_Power * 0.35f;    

            if (pc.target.GetComponent<PlayerController>() != null) {
                pc.target.GetComponent<PlayerController>().entity.DealDamage(amount);
            } else if (pc.target.GetComponent<EnemyController>() != null) {
                pc.target.GetComponent<EnemyController>().entity.DealDamage(amount);
            } else if (pc.target.GetComponent<BossController>() != null) {
                pc.target.GetComponent<BossController>().entity.DealDamage(amount);
            }

            // Alternate Attack Animation
            if (pc.characterClass == CharacterClass.Rogue) {
                animator.SetBool("Alternate Attack", !animator.GetBool("Alternate Attack"));
            }

            // Play SFX
            audioSource.PlayOneShot(pc.GetCharacter().Sounds["attack"], 0.5f);
        }

        
    }

    public void AbilityOne()
    {   
        // Generate the ability settings
        AbilitySettings settings;
        settings.owner = pc.entity;
        
        if (pc.target.GetComponent<PlayerController>() != null) {
            settings.target = pc.target.GetComponent<PlayerController>().entity;
        } else if (pc.target.GetComponent<EnemyController>() != null) {
            settings.target = pc.target.GetComponent<EnemyController>().entity;
        } else if (pc.target.GetComponent<BossController>() != null) {
            settings.target = pc.target.GetComponent<BossController>().entity;
        } else {
            settings.target = null;
        }
        // Cast the Ability
        pc.GetCharacter().A1(settings);
    }

    public void AbilityTwo()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        
        if (pc.target.GetComponent<PlayerController>() != null) {
            settings.target = pc.target.GetComponent<PlayerController>().entity;
        } else if (pc.target.GetComponent<EnemyController>() != null) {
            settings.target = pc.target.GetComponent<EnemyController>().entity;
        } else if (pc.target.GetComponent<BossController>() != null) {
            settings.target = pc.target.GetComponent<BossController>().entity;
        } else {
            settings.target = null;
        }


        // Cast the Ability
        pc.GetCharacter().A2(settings);
    }

    public void AbilityThree()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        
        if (pc.target.GetComponent<PlayerController>() != null) {
            settings.target = pc.target.GetComponent<PlayerController>().entity;
        } else if (pc.target.GetComponent<EnemyController>() != null) {
            settings.target = pc.target.GetComponent<EnemyController>().entity;
        } else if (pc.target.GetComponent<BossController>() != null) {
            settings.target = pc.target.GetComponent<BossController>().entity;
        } else {
            settings.target = null;
        }
        // Cast the Ability
        pc.GetCharacter().A3(settings);
    }

    public void AbilityFour()
    {
        AbilitySettings settings;
        settings.owner = pc.entity;
        
        if (pc.target.GetComponent<PlayerController>() != null) {
            settings.target = pc.target.GetComponent<PlayerController>().entity;
        } else if (pc.target.GetComponent<EnemyController>() != null) {
            settings.target = pc.target.GetComponent<EnemyController>().entity;
        } else if (pc.target.GetComponent<BossController>() != null) {
            settings.target = pc.target.GetComponent<BossController>().entity;
        } else {
            settings.target = null;
        }

        // Cast the Ability
        pc.GetCharacter().A4(settings);
    }

    public void Investigate()
    {
        
        pc.Investigate();
        pc.target.GetComponent<InteractionData>().hasBeenInteractedWith = true;

        pc.SetAction(PlayerController.ACTION.INVESTIGATING);
    }
}
