using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawns;
    public Wave[] waves;
    public int currentWave = 0;
    public List<EnemyMovement> activeEnemies = new List<EnemyMovement>();

    [System.Serializable]
    public enum WaveStartCondition
    {
        AllEnemiesKilled,
        OnPreviousWaveStart,
    }

    [System.Serializable]
    public class Wave
    {
        public WaveStartCondition startCondition;
        public float startDelay = 0;
        public float delayBetweenObjectSpawns = 0;
        public int spawnerIncrement = 1;
        public bool addEnemiesToEnemyPool = true;
        public ObjectSpawn[] objectsToSpawn;
    }

    [System.Serializable]
    public class ObjectSpawn
    {
        public GameObject spawnObject;
        public int quantity = 1;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (currentWave < waves.Length)
        {
            var wave = waves[currentWave];
            var currentSpawner = Random.Range(0, spawns.Length);
            var increment = (Random.Range(0, 2) * 2 - 1) * wave.spawnerIncrement;
            yield return new WaitForSeconds(wave.startDelay);
            for (int i = 0; i < wave.objectsToSpawn.Length; i++)
            {
                var obj = wave.objectsToSpawn[i];
                for (int j = 0; j < obj.quantity; j++)
                {
                    var spawner = spawns[currentSpawner];
                    var spawnedObject = Instantiate(obj.spawnObject, spawner.transform.position, Quaternion.Euler(0, 0, 0));
                    var enemy = spawnedObject.GetComponent<EnemyMovement>();
                    if (enemy != null && wave.addEnemiesToEnemyPool)
                    {
                        activeEnemies.Add(enemy);
                    }
                    if (wave.delayBetweenObjectSpawns > 0)
                    {
                        yield return new WaitForSeconds(wave.delayBetweenObjectSpawns);
                    }
                    currentSpawner += increment;
                    if (currentSpawner < 0)
                    {
                        currentSpawner = spawns.Length - 1;
                    } else if (currentSpawner >= spawns.Length)
                    {
                        currentSpawner = 0;
                    }
                }
            }

            switch (wave.startCondition)
            {
                case WaveStartCondition.AllEnemiesKilled:
                    var anyEnemyAlive = false;
                    do {
                        anyEnemyAlive = false;

                        foreach (var enemy in activeEnemies)
                        {
                            if (enemy != null)
                            {
                                anyEnemyAlive = true;
                            }
                        }
                        
                        yield return null;
                    } while (anyEnemyAlive);

                    activeEnemies.Clear();
                    break;
                case WaveStartCondition.OnPreviousWaveStart:
                    break;
            }

            currentWave += 1;
        }
        yield return null;
    }
}
