using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public NepritelPOHYB enemy;
   
    Transform target;

    public int maxNumberOfEnemies;
    public int numberOfEnemiesAlive;
    public float timeBetweenSpawns;

    public bool spawnenemy;

    float nextSpawnTime;

    HracSTATY hrcStaty;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("HRAC") != null)
        {
            target = GameObject.FindGameObjectWithTag("HRAC").transform;
            hrcStaty = target.GetComponent<HracSTATY>();

        }
    }

    void Update()
    {

        if (hrcStaty.fuelSlider.value == 10)
        {
            maxNumberOfEnemies = 20;
        }else if (hrcStaty.fuelSlider.value == 20)
        {
            maxNumberOfEnemies = 30;
        }
        else if(hrcStaty.fuelSlider.value == 30)
        {
            maxNumberOfEnemies = 40;
        }else if (hrcStaty.fuelSlider.value == 40)
        {
            maxNumberOfEnemies = 100;
        }
        


  if (numberOfEnemiesAlive == 0 || numberOfEnemiesAlive < maxNumberOfEnemies)
        {
            spawnenemy = true;
        }

        if (spawnenemy && Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            NepritelPOHYB spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity) as NepritelPOHYB;
            numberOfEnemiesAlive++;
            spawnedEnemy.OnDeath += OnEnemyDeath;

        }


        if (numberOfEnemiesAlive == maxNumberOfEnemies)
        {
            spawnenemy = false;
        }

    }

    void OnEnemyDeath()
    {
        numberOfEnemiesAlive--;
    }

}

