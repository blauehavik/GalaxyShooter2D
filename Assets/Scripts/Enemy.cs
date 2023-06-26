using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player null in Enemy.cs");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator null in Enemy.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLasers();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, 0);
        }

    }
    private void FireLasers()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger!" + other.gameObject.tag);

        if(other.gameObject.tag == "Laser")
        {
            //Debug.Log("Hit with Laser");
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            Destroy(other.gameObject);
            DestroyEnemy();
        }
        else if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Hit with Player");

            Player player = other.GetComponent<Player>();

            player.Damage();
            DestroyEnemy();
        }
    }
    private void DestroyEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionSound;
        _audioSource.Play();

        _speed = 0f;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }
}
