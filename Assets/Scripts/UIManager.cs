using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites; //allows us to create list of sprites for each life so can access based on lives left
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    public int _bestScore, _score;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        _bestScoreText.text = "Best Score: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>(); //to access game manager script to restart scene

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _score += 10;
        _scoreText.text = "Score: " + playerScore;
    }

    public void CheckForBestScore()
    {
        if(_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("Best Score", _bestScore);
            _bestScoreText.text = "Best Score: " + _bestScore;

        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives]; //lives sprites in unity has sprite options for 0-4 lives
                                                        //method allows lives count on player to access sprites
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());

#if UNITY_ANDROID
        _restartText.text = "PRESS FIRE TO RESTART";
        _restartText.gameObject.SetActive(true);
#endif

        _restartText.gameObject.SetActive(true);
    }


    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
