using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    
    public bool isTwoPlayer = false;

    [SerializeField]
    private GameObject _pauseMenu;

    private Animator _pauseAnimator;


    private void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    private void Update()
    {
        if (_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            if (isTwoPlayer == true)
            {
                SceneManager.LoadScene("Co-Op_Mode");
            }
            else if(isTwoPlayer == false)
            {
                SceneManager.LoadScene("Single_Player");
            }

        }
#if UNITY_ANDROID
        else if(_isGameOver == true && CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            SceneManager.LoadScene(1);
        }
#endif

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("New_Main_Menu");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
        }
    }

    public void GameOver()
    {
        _isGameOver = true; 
    }    

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("New_Main_Menu");
    }
}
