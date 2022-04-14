using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEventHandler : MonoBehaviour
{
    private BossController bc;
    private Animator animator;
    private AudioSource audioSource;


    public bool DebugMe = false;

    private void Awake()
    {
        bc = GetComponentInParent<BossController>();
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(string str) 
    {
        //Debug.Log(str);

        //audioSource.PlayOneShot(Resources.Load<AudioClip>(str));
    }

    public void Attack()
    {
        if (DebugMe) Debug.Log("Attack() called");

        // Check if target exists
        if (bc.target != null) {            
        
            // Deal Damage to Target
            float amount = bc.stats.Attack_Power * 1.0f;

            if (bc.target.GetComponent<PlayerController>() != null) {
                bc.target.GetComponent<PlayerController>().entity.DealDamage(amount);
            }
        }        
    }

    public void Toss()
    {
        bc.TossAttack();
    }

    public void Roar()
    {
        bc.Roar();
    }

    public void Powerup()
    {
        bc.Powerup();
    }
}
