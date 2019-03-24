using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    [Space]

    public GameObject ant;
    public GameObject beetle;

    public float respawnRate = 4f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", respawnRate, 1f);
    }

    void SpawnEnemy() {
        int randomNumber = Random.Range(0, 1);
        if (randomNumber == 0){
            Instantiate(ant, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
        else if (randomNumber == 1){
            return;
        }
        
    }
}
