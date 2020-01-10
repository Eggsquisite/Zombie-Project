using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefab;
    [SerializeField] float spawnRate = 5f;
    private float spawnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        spawnTimer += Time.time;
        if (spawnTimer >= spawnRate)
        {
            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count)], transform.position, Quaternion.identity);
            spawnTimer = 0;
            spawnRate += Time.time;
            Debug.Log("here" + Time.time);
        }
    }
}
