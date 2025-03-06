using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed;
    public Transform closestEnemy;
    public GameObject[] multipleEnemies;

    // Update is called once per frame
    void Update()
    {
        closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            ChasingEnemy();
        }
    }

    public void ChasingEnemy()
    {
        Vector2 lookDirection = (closestEnemy.position - transform.position);
        transform.up = new Vector2(lookDirection.x, lookDirection.y);

        transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, speed * Time.deltaTime);
    }

    public Transform GetClosestEnemy()
    {
        // Get all enemies with the tag "Enemy"
        multipleEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Find the boss using its tag
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        // Combine enemies and boss into a single list
        List<GameObject> allTargets = new List<GameObject>(multipleEnemies);
        if (boss != null) // Check if the boss exists
        {
            allTargets.Add(boss);
        }

        // Get the closest target
        float closestDistance = Mathf.Infinity;
        Transform enemyPos = null;

        // Search through all targets to find the closest one
        foreach (GameObject target in allTargets)
        {
            float currentDistance = Vector3.Distance(transform.position, target.transform.position);

            // If the current distance is less than the closest distance
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                enemyPos = target.transform; // Update the closest target
            }
        }

        return enemyPos; // Return the closest enemy or boss
    }
}
