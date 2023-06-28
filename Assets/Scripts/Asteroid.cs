using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").
            GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed *
            Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            GameObject newEnemy = Instantiate(_explosionPrefab,
                transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, .25f);
        }
    }
}