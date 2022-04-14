using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    AbilitySettings settings;

    bool isFlying = true;
    float flyTimer = 0.0f;

    float boulderDistance = 25.0f;

    Vector3 startPosition;
    Vector3 endPosition;

    private void Awake()
    {
        startPosition = transform.position;
        endPosition = startPosition + transform.forward * boulderDistance;
    }

    public void Initialize(AbilitySettings _settings)
    {
        settings = _settings;
        
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(startPosition, endPosition, flyTimer);

        if (isFlying)
        {
            flyTimer += Time.deltaTime;            

            if (flyTimer > 1) {
                flyTimer = 1; 
                isFlying = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.tag == "Player")
        {            
            float amount = 15.0f;
            other.GetComponent<PlayerController>().entity.DealDamage(amount);            
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {   
            // Start Position
            Gizmos.DrawSphere(startPosition, 1.0f);

            // End Position
            if (Application.isPlaying)
            {
                Gizmos.DrawSphere(endPosition, 1.0f);
            }
        }
    }
}
