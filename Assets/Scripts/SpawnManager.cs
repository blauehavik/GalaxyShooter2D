using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private int _wave;
    [SerializeField]
    private int _enemiesKilledCount;
    [SerializeField]
    UIManager _uiManager;

    public struct EnemyData
    {
        public int count;
        public float speed;
        public float fireRate;
        public EnemyData(int count, float speed, float fireRate)
        {
            this.count = count;
            this.speed = speed;
            this.fireRate = fireRate;
        }
    } ;
    [SerializeField]
    EnemyData[] _waves = { new EnemyData(2,1f,10f),
        new EnemyData(4,2f,5f),
        new EnemyData(5,3f,8f),
        new EnemyData(18,4f,8f),
        new EnemyData(22,5f,6f),
        new EnemyData(26,6f,5f),
        new EnemyData(30,7f,4f),
        new EnemyData(34,8f,3f),
        new EnemyData(38,9f,2f),
        new EnemyData(42, 10f, 1f) };

public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            if (_enemiesKilledCount <= _waves[_wave].count)
            {
                GameObject newEnemy;
                float randomX = Random.Range(-8f, 8f);
                newEnemy = Instantiate(_enemyPrefab[1],
                   new Vector3(randomX, 8f, 0), Quaternion.identity);

                /*if (Random.Range(0f,1f) > .1f)
                {
                     newEnemy = Instantiate(_enemyPrefab,
                        new Vector3(randomX, 8f, 0), Quaternion.identity);
                }
                else
                {
                     newEnemy = Instantiate(_enemyPrefab,
                        new Vector3(randomX, 8f, 0), Quaternion.Euler(
                        new Vector3(0, 0, Random.Range(-45, 46))));
                }
                */
                Enemy enemy = newEnemy.GetComponent<Enemy>();
                enemy.SetSpeed(_waves[_wave].speed);
                enemy.SetFireRate(_waves[_wave].fireRate);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
       }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            int randomPowerup = Random.Range(0, _powerups.Length);
            //randomPowerup = 4;
            GameObject newPowerup = Instantiate(_powerups[randomPowerup],
                new Vector3(randomX, 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(15f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            int randomPowerup = Random.Range(0, _rarePowerups.Length);
            GameObject newPowerup = Instantiate(_rarePowerups[randomPowerup],
                new Vector3(randomX, 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void OnPlayerDied()
    {
        _stopSpawning = true;
    }

    public void OnEnemyDied()
    {
        if (_enemiesKilledCount++ >= _waves[_wave].count)
        {
            _wave++;
            _stopSpawning = false;
            _enemiesKilledCount = 0;
            _uiManager.UpdateWave(_wave+1);
            
        }
    }
}
