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
    private TMP_Text _ammoText;
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

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _scoreText.text = "Score: 0";
        _gameover_Text.gameObject.SetActive(false);
        _restart_Text.gameObject.SetActive(false);

        if (_gameManager == null)
        {
            Debug.LogError("GameManager os NULL:.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateAmmoCount(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
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
