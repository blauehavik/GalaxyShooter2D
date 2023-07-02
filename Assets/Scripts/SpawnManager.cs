using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 8f));
        while (_stopSpawning == false)
        {
            GameObject newEnemy;
            float randomX = Random.Range(-8f, 8f);
            if (Random.Range(0f,1f) > .1f)
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
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
       }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 8f));
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            int randomPowerup = Random.Range(0, _powerups.Length);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup],
                new Vector3(randomX, 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(Random.Range(15f, 25f));
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
}
