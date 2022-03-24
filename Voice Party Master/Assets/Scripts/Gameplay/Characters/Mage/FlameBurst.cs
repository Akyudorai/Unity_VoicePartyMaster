using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    
    ParticleSystem ps;
    public List<string> targetTags = new List<string>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    public List<GameObject> targetsHit = new List<GameObject>();
    AbilitySettings settings;


    public void Initialize(AbilitySettings _settings) 
    {
        settings = _settings;
        gameObject.SetActive(true);
    }

    void OnEnable() 
    {
        // Get the ParticleSystem component from gameobject.
        ps = GetComponent<ParticleSystem>();

        if (ps == null) {
            Debug.Log("FlameBurst::Could not find ParticleSystem on " + gameObject.name);
        }

        // Create a counter so we can add each collider on its own index later
        int index = 0;

        // Look for all objects with the target collision tag
        for(int i = 0 ; i < targetTags.Count; i++)
        {
            var allColliderGameObjects = GameObject.FindGameObjectsWithTag(targetTags[i]);

            // Now look at each collider game object that we found
            foreach (GameObject colliderObject in allColliderGameObjects)
            {
                // Try to get the collider component from the game object
                Collider col = colliderObject.GetComponent<Collider>();

                // If we can't find, display an error
                if (col == null) 
                {
                    Debug.Log("FlameBurst::Could not find collider component on " + colliderObject.name);                
                }

                else 
                {
                    // Add the collider to the particle system.
                    ps.trigger.SetCollider(index, col);
                    index++;
                }
            } 
        }
    }    

    void OnParticleTrigger() 
    {         
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside, out var insideData);

        for (int i = 0; i < numInside; i++)
        {
            for (int j = 0; j < insideData.GetColliderCount(i); j++) 
            {
                GameObject go = insideData.GetCollider(i,j).gameObject;
                if (!targetsHit.Contains(go)) 
                {
                    // Add object to list so it doesn't get triggered multiple times.
                    targetsHit.Add(go);
                    
                    // Calculate Power
                    float amount = settings.owner.GetOwner().GetCharacter().GetStats().Spell_Power * 2.25f;

                    // Damage all targets with the enemy tag
                    if (go.tag == "Enemy") 
                    {
                        // Deal Damage to the Target based on Spell Power                                     
                        go.GetComponent<EnemyController>().entity.DealDamage(amount);                            
                    }                  
                }
            }
        }
    }
}
