using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    private float currentHealth, maxHealth;

    public Entity(float maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;        
    }

    public void DealDamage(float amount) {
        
        if (currentHealth - amount <= 0) {
            // Trigger Death
            Debug.Log("Death");            
        }
        
        currentHealth -= amount;
    }
}
