using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField]
    private float _speed = 5.0f;
    private int _score = 0;
    private float _cooldownTime = .2f;
    private float _nextFireTime = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    //[SerializeField]
    //private CameraEffects _cameraEffects;
    public CameraShake _cameraShake;

    [Header("Manager Variables")]
    [SerializeField]
    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    [Header("Powerup Variables")]
    private bool _isTripleShotActive = false;
    private bool _isSuperShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isSlowActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private float _speedMultiplier = 2.0f;
    private int _shieldLevel = 0;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private SpriteRenderer _shieldSpriteRenderer;
    private Color[] _spriteColor = new Color[]
        {
            new Color (1,1,1,.25f), new Color (1,1,1,.5f),
            new Color (1,1,1,.75f), new Color (1,1,1,1f)
        };
    [SerializeField]
    private float _tripleShotResetTime = 5.0f;
    [SerializeField]
    private float _superShotResetTime = 5.0f;
    [SerializeField] 
    private float _speedBoostResetTime = 5.0f;

    [Header("Thruster Variables")]
    [SerializeField]
    private GameObject _thruster;
    private float _thrustMultiplier = 2.0f;
    private int _thrustFuel, _thrustMax = 200;

    [Header("Laser Variables")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _laserEmptyAudio;
    private AudioSource _audioSource;
    private int _ammoCount = 15;
    private int _maxAmmo = 15;

    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldSpriteRenderer = _shieldVisualizer.
            GetComponent<SpriteRenderer>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }
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
        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("Shield SpriteRenderer is null.");
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
        if (Input.GetKey(KeyCode.LeftShift) && _thrustFuel > 0)
        {
            totalSpeed *= _thrustMultiplier;
            _thruster.transform.localScale = thrusterScale;
            _thrustFuel -= 2;
            _uiManager.UpdateThrustCount(
                (float)_thrustFuel/(float)_thrustMax);
        }
        else
        {
            if (_thrustFuel <= _thrustMax)
            {
                _uiManager.UpdateThrustCount(
                     (float)_thrustFuel++ /(float)_thrustMax);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thruster.transform.localScale = Vector3.one;
        }
        if (_isSlowActive == true)
        {
            totalSpeed *= .3f;
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
            if (_isTripleShotActive == true && _ammoCount >= 3)
            {
                Instantiate(_tripleShotPrefab,
                    transform.position, Quaternion.identity);
                _ammoCount -= 3;
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
                _uiManager.UpdateAmmoCount(_ammoCount);
                return;
            }
            else if (_isSuperShotActive == true)
            {
                Instantiate(_laserPrefab, transform.position,
                    Quaternion.Euler(new Vector3(0, 0, 0)));
                Instantiate(_laserPrefab, transform.position,
                    Quaternion.Euler(new Vector3(0, 0, 45)));
                Instantiate(_laserPrefab, transform.position,
                    Quaternion.Euler(new Vector3(0, 0, -45)));
                Instantiate(_laserPrefab, transform.position,
                    Quaternion.Euler(new Vector3(0, 0, 30)));
                Instantiate(_laserPrefab, transform.position,
                    Quaternion.Euler(new Vector3(0, 0, -30)));
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
                return;
            }
            else if (_ammoCount >= 1)
            {
                Instantiate(_laserPrefab,
                    transform.position + new Vector3(0, 0.75f, 0),
                    Quaternion.identity);
                _ammoCount--;
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
                _uiManager.UpdateAmmoCount(_ammoCount);
                return;
            }
            _audioSource.clip = _laserEmptyAudio; ;
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_shieldLevel > 0)
        {
            _shieldLevel--;
            _shieldSpriteRenderer.color =
                    _spriteColor[_shieldLevel];
            if (_shieldLevel <= 0)
            {
                _shieldVisualizer.SetActive(false);
            }
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
        StartCoroutine(_cameraShake.Shake(.3f, .5f));
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDied();
            Destroy(this.gameObject, .3f);
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

    public void SuperShotActive()
    {
        _isSuperShotActive = true;
        StartCoroutine(SuperShotCooldownRoutine());
    }

    IEnumerator SuperShotCooldownRoutine()
    {
        yield return new WaitForSeconds(_superShotResetTime);
        _isSuperShotActive = false;
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
    public void SlowActive()
    {
        _isSlowActive = true;
        StartCoroutine(SlowCooldownRoutine());
    }

    IEnumerator SlowCooldownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostResetTime*2f);
        _isSlowActive = false;

    }

    public void ShieldsActive()
    {
        if (_shieldLevel < 3)
        {
            _shieldLevel++;
        }
        _shieldVisualizer.SetActive(true);
        _shieldSpriteRenderer.color = _spriteColor[_shieldLevel];
    }

    public void AmmoReload()
    {
        _ammoCount = _maxAmmo;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    public void HealthBoost()
    {
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 3)
            {
                _leftEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _rightEngine.SetActive(false);
            }
            _uiManager.UpdateLives(_lives);
        }
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}