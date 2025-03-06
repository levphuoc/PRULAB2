using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LifeController
{
    public int specialAttackDamage = 5; // Example of a boss-specific variable
    public float specialAttackCooldown = 3f;
    private float lastSpecialAttackTime;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // You can override TakeDamage if needed
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        // Additional logic for the boss taking damage
        Debug.Log("Boss takes damage! Remaining health: " + health);

        // Check if the boss is dead and handle win condition
        if (isDead)
        {
            GameManager.instance.GameWon(); // Call win game logic from GameManager
        }
    }
}
