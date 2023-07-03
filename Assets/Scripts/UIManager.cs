using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _thrustText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private TMP_Text _gameover_Text;
    [SerializeField]
    private TMP_Text _restart_Text;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Slider _thrustSlider;

    [Header("Ammo")]
    [SerializeField]
    private TMP_Text _ammoText;
    [SerializeField]
    private GameObject _ammoContainer;
    [SerializeField]
    private Image[] _ammoImages;
    [SerializeField]
    private Sprite _ammoFull;
    [SerializeField]
    private Sprite _ammoEmpty;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _ammoText.text = "Ammo: 15";
        _thrustText.text = "Thrust:";
        _thrustSlider.value = 0f;
        _gameover_Text.gameObject.SetActive(false);
        _restart_Text.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager os NULL:.");
        }

        _ammoImages =
            _ammoContainer.GetComponentsInChildren<Image>();
        Debug.Log(_ammoImages.Length);
        if (_ammoImages == null)
        {
            Debug.LogError("Ammo sprites is null!");
            //Debug.Break();
        }

    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmoCount(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;

        for (int i = 0; i < _ammoImages.Length; i++)
        {
            if (i < ammo)
            {
                _ammoImages[i].sprite = _ammoFull;
            }
            else
            {
                _ammoImages[i].sprite = _ammoEmpty;
            }
        }
    }

    public void UpdateThrustCount(float thrust)
    {
        _thrustSlider.value = thrust;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameover_Text.gameObject.SetActive(true);
        _restart_Text.gameObject.SetActive(true);
        StartCoroutine(FlashGameOver());
    }

    IEnumerator  FlashGameOver()
    {
        float flashTime = .05f;
        while (true)
        {
            yield return new WaitForSeconds(flashTime);
            if (flashTime <= 1.0f)
            {
                flashTime += .025f;
            }
            if (_gameover_Text.gameObject.activeSelf)
            {
                _gameover_Text.gameObject.SetActive(false);
            }
            else
            {
                _gameover_Text.gameObject.SetActive(true);
            }
        }
    }
}
