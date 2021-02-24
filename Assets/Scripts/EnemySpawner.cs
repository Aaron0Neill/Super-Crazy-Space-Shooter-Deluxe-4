using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    Transform _playerTransform;

    int _waveNumber = 1;
    int _spawnedEnemies = 0;
    string _sceneName;

    void Awake()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnWave(_waveNumber++));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            if (_spawnedEnemies >= _waveNumber + 3)
            {
                _spawnedEnemies = 0;
                StartCoroutine(SpawnWave(_waveNumber++));
            }
        }
    }

    IEnumerator SpawnWave(int t_waveNumber)
    {
        while (_spawnedEnemies <= t_waveNumber + 3)
        {
            Vector3 spawnPos = new Vector3();
            if (_sceneName == "Level1")
            { // level 1 controlled spawning
                spawnPos = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                float closestdistance = Vector3.Distance(spawnPos, _playerTransform.position);
                float currentTime = Time.time + 1.0f;
                while (closestdistance < 3.0f || currentTime < Time.time)
                { // keep looping until the spawn point is far enough away from the player with a time limit
                    spawnPos = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                    closestdistance = Vector3.Distance(spawnPos, _playerTransform.position);
                    for (int i = 0; i < transform.childCount; ++i)
                    { // try not spawn close to other enemies
                        float d = Vector3.Distance(spawnPos, transform.GetChild(i).transform.position);
                        if (d < closestdistance)
                            closestdistance = d;
                    }
                }
            }

            if (_sceneName == "Level2")
            {
                //get the degree rotation on each angle
                Vector3 rotation = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
                spawnPos = Quaternion.Euler(rotation) * new Vector3(0, -20, 0);
                spawnPos += new Vector3(0, -20, 0);
            }
            

            
            Instantiate(enemy, spawnPos, Quaternion.identity, this.transform);
            _spawnedEnemies++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
