using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCaller : MonoBehaviour
{
    public float invincibilityDuration = 5f;  // Invincible duration in seconds
    private bool isInvincible = false;
    public Renderer bossCallerRenderer;  // Reference to the BossCaller's renderer

    // This method simulates the BossCaller taking damage
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            Debug.Log("BossCaller took damage: " + damage);
            StartCoroutine(ActivateInvincibility());
        }
        else
        {
            Debug.Log("BossCaller is invincible and can't take damage.");
        }
    }

    // Coroutine to activate invincibility for a limited duration
    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        Debug.Log("BossCaller is now invincible.");

        // Flash effect
        for (int i = 0; i < 5; i++)
        {
            bossCallerRenderer.enabled = false;  // Hide the object
            yield return new WaitForSeconds(0.2f);
            bossCallerRenderer.enabled = true;   // Show the object
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(invincibilityDuration); // Wait for the invincibility duration
        isInvincible = false;
        Debug.Log("BossCaller is no longer invincible.");
    }

    // Method to check if the BossCaller is currently invincible
    public bool IsInvincible()
    {
        return isInvincible;
    }
}
