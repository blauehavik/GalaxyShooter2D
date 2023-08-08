using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Variables")]
    [SerializeField]
    protected float _speed = 4.0f;
    protected Player _player;
    [SerializeField]
    protected SpawnManager _spawnManager;
    protected Animator _animator;
    [SerializeField]
    protected AudioClip _explosionSound;
    protected AudioSource _audioSource;
    protected bool _isShieldOn;
    [SerializeField]
    protected GameObject _shieldVisualizer;
    protected SpriteRenderer _shieldSpriteRenderer;
    protected Color[] _spriteColor = new Color[]
        {
            new Color (1,.5f,0,1f)
        };

    [Header("Laser Variables")]
    [SerializeField]
    protected GameObject _laserPrefab;
    protected float _fireRate = 3.0f;
    protected float _canFire = -1.0f;


    protected virtual void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _shieldSpriteRenderer = _shieldVisualizer.
            GetComponent<SpriteRenderer>();

        if (_player == null)
        {
            Debug.LogError("Player null in Enemy.cs");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator null in Enemy.cs");
        }
        _spawnManager = GameObject.Find("Spawn_Manager")
            .GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("Shield SpriteRenderer is null.");
        }
        _shieldSpriteRenderer.color = _spriteColor[0];
        if (Random.Range(0f, 1f) > .1f)
        {
            SetShield();
        }

    }

    void Update()
    {
        CalculateMovement();
        FireWeapon();
    }

    protected virtual void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    protected virtual void  FireWeapon()
    {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab,
                transform.position, transform.rotation);// Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            Destroy(other.gameObject);
            DestroyEnemy();
        }
        else if (other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            player.Damage();
            DestroyEnemy();
        }
    }

    protected void DestroyEnemy()
    {
        if (_isShieldOn)
        {
            _isShieldOn = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _animator.SetTrigger("OnEnemyDeath");
        _spawnManager.OnEnemyDied();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
        _speed = 0f;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetFireRate(float fireRate)
    {
        _fireRate = fireRate;
    }

    public void SetShield(bool isShieldOn = true)
    {
        _isShieldOn = isShieldOn;
        _shieldVisualizer.SetActive(true);

    }
}