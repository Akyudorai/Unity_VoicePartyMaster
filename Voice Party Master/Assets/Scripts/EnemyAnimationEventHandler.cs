using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    private EnemyController ec;
    private Animator animator;
    private AudioSource audioSource;


    public bool DebugMe = false;

    private void Awake()
    {
        ec = GetComponentInParent<EnemyController>();
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
        if (ec.target != null) {            
        
            // Deal Damage to Target
            float amount = ec.stats.Attack_Power * 1.0f;

            if (ec.target.GetComponent<PlayerController>() != null) {
                ec.target.GetComponent<PlayerController>().entity.DealDamage(amount);
            }
        }

        
    }
}
