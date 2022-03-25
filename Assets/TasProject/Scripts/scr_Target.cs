using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Target : MonoBehaviour { 

    public float health = 50f;
    [SerializeField] public int damageAmount = 20;

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




    private void OnCollisionEnter(Collision collision)
    {
        scr_DieCharacter damagedPlayer = collision.gameObject.GetComponent<scr_DieCharacter>();

        if (damagedPlayer != null)
        {

            damagedPlayer.DealDamage(damageAmount);

        }

        if (collision.gameObject.name == "Player")

       print(collision.gameObject.name + "colliding with object");
    }

}

