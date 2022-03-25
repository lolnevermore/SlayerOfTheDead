using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Target : MonoBehaviour { 

    public float health = 50f;
    
    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

        
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}