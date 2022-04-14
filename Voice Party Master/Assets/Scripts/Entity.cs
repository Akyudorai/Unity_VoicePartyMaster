using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity
{
    public bool IsDead;
    private float currentHealth, maxHealth;
    Animator animator;
    PlayerController pc;

    public GameObject healthPanel;
    public Image healthBar;

    public delegate void OnDeathEvent();
    public delegate void OnDamageReceived();
    public OnDamageReceived onDamageReceived;
    public OnDeathEvent onDeath;

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
            currentHealth = 0;
            
            if (healthPanel != null) {
                healthPanel.SetActive(false);
            }
            
            animator.SetTrigger("Death");

            if (onDeath != null) {
                onDeath.Invoke();
            }
        } else {
            currentHealth -= amount;
        }
        
        // Adjust Healthbar
        if (healthBar != null) {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        

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
