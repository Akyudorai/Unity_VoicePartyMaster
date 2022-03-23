using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public bool IsDead;
    private float currentHealth, maxHealth;
    Animator animator;
    PlayerController pc;

    public delegate void OnDamageReceived();
    public OnDamageReceived onDamageReceived;

    public Entity(float maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;        

        IsDead = false;
        //onDamageReceived += DebugDelegate;
    }

    public float GetCurrentHealth() { return currentHealth; }
    public void SetCurrentHealth(float amount) { currentHealth = amount; }
    public float GetMaxHealth() { return maxHealth; }

    public Animator GetAnimator() { return animator; }
    public void SetAnimator(Animator reference) { animator = reference; }

    public PlayerController GetOwner() { return pc; }
    public void SetOwner(PlayerController controller) { pc = controller; }

    public void DealDamage(float amount) {
        
        if (IsDead) return;

        if (currentHealth - amount <= 0) {
            IsDead = true;
            animator.SetTrigger("Death");
        }
        
        currentHealth -= amount;

        // Invoke OnDamageReceived Event
        if (onDamageReceived != null) {
            onDamageReceived.Invoke();
        }
    }

    private void DebugDelegate()
    {
        Debug.Log("OnDamageReceived Called!");
    }
}
