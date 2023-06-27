using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2.0f;
    private float _thrustMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _cooldownTime = .2f;
    private float _nextFireTime = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private GameObject _thruster;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject shieldVisualizer;

    [SerializeField]
    private float _tripleShotResetTime = 5.0f;
    [SerializeField]
    private float _speedBoostResetTime = 5.0f;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL.");
        }else
        {
            _audioSource.clip = _laserAudio;
        }
    }

    void Update()
    {
        MovePlayer();
        FireLaser();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float totalSpeed = _speed;
        Vector3 thrusterScale = new Vector3(1f, 1.5f, 1f);

        if (_isSpeedBoostActive == true)
        {
            totalSpeed *= _speedMultiplier;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalSpeed *= _thrustMultiplier;
            _thruster.transform.localScale = thrusterScale;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thruster.transform.localScale = Vector3.one;
        }
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * totalSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 0), 0);

        if (transform.position.x >= 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + _cooldownTime;
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
            }
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        //Debug.Log("Player lives: " + _lives);
        if (_lives <1)
        {
            _spawnManager.OnPlayerDied();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCooldownRoutine());
    }

    IEnumerator TripleShotCooldownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotResetTime);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostCooldownRoutine());
    }

    IEnumerator SpeedBoostCooldownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostResetTime);
        _isSpeedBoostActive = false;

    }
    public void ShieldsActive()
    {
        _isShieldActive = true;
        shieldVisualizer.SetActive(true);
    }
    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}