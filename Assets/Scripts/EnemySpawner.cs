using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefab;
    [SerializeField] float spawnRateMin = 2f;
    [SerializeField] float spawnRateMax = 3f;
    [SerializeField] float spawnAmount = 5f;
    bool spawn = true;
    float enemiesSpawned = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawn()
    {
        do
        {
            Debug.Log("Hello");
            if (enemiesSpawned >= spawnAmount)
                spawn = false;

            yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));

            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count)], transform.position, Quaternion.identity);
            enemiesSpawned++;

        } while (spawn);
    }
}
