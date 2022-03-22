using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public bool IsDead;
    private float currentHealth, maxHealth;
    Animator animator;

    public Entity(float maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;        

        IsDead = false;
    }

    public void SetAnimator(Animator reference)
    {
        animator = reference;
    }

    public void DealDamage(float amount) {
        
        if (IsDead) return;

        if (currentHealth - amount <= 0) {
            IsDead = true;
            animator.SetTrigger("Death");
        }
        
        currentHealth -= amount;
    }
}
