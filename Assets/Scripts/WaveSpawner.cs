using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawns;
    public Transform[] centerSpawns;
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
        public bool useCenterSpawns = false;
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
            switch (wave.startCondition)
            {
                case WaveStartCondition.AllEnemiesKilled:
                    var anyEnemyAlive = false;
                    do
                    {
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
            var currentSpawner = UnityEngine.Random.Range(0, spawns.Length);
            var currentCenterSpawner = UnityEngine.Random.Range(0, centerSpawns.Length);
            var increment = (UnityEngine.Random.Range(0, 2) * 2 - 1) * wave.spawnerIncrement;
            yield return new WaitForSeconds(wave.startDelay);
            for (int i = 0; i < wave.objectsToSpawn.Length; i++)
            {
                var obj = wave.objectsToSpawn[i];
                for (int j = 0; j < obj.quantity; j++)
                {
                    Transform spawner;
                    currentCenterSpawner = Math.Min(currentCenterSpawner, centerSpawns.Length - 2);
                    currentSpawner = Math.Min(currentSpawner, spawns.Length - 2);
                    if (obj.useCenterSpawns) spawner = centerSpawns[currentCenterSpawner];
                    else spawner = spawns[currentSpawner];

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
                    var die = spawnedObject.GetComponent<Die>();
                    if (die != null) die.MakePickup(2);
                    currentSpawner += increment;
                    if (obj.useCenterSpawns)
                    {
                        if (currentCenterSpawner < 0)
                        {
                            currentCenterSpawner = centerSpawns.Length - 1;
                        }
                        else if (currentCenterSpawner >= centerSpawns.Length)
                        {
                            currentCenterSpawner = 0;
                        }
                    } else
                    {
                        if (currentSpawner < 0)
                        {
                            currentSpawner = spawns.Length - 1;
                        }
                        else if (currentSpawner >= spawns.Length)
                        {
                            currentSpawner = 0;
                        }
                    }
                }
            }

            currentWave += 1;
        }

        {
            var anyEnemyAlive2 = false;
            do
            {
                anyEnemyAlive2 = false;

                foreach (var enemy in activeEnemies)
                {
                    if (enemy != null)
                    {
                        anyEnemyAlive2 = true;
                    }
                }

                yield return null;
            } while (anyEnemyAlive2);
        }


        Debug.Log("All waves clear!");
        SceneManager.LoadScene("Scenes/WinScene");
        yield return null;
    }
}
