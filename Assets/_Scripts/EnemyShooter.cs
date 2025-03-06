using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate;
    public Transform[] shotSpawn;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", fireRate, fireRate);
    }

    // Update is called once per frame
    void Fire()
    {
        for (int i = 0; i < shotSpawn.Length; i++)
        {
            Instantiate(bullet, shotSpawn[i].position, shotSpawn[i].rotation);
        }
    }
}
