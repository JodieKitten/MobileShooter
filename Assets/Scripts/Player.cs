using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldVisualiser;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _thruster;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioSource _LaserAudioSource;

    [SerializeField]
    private AudioSource _ExplosionAudioSource;

    [SerializeField]
    private AudioClip _LaserAudioClip;

    [SerializeField]
    private AudioClip _ExplosionClip;

    private bool _isTripleShotEnabled = false;

    private bool _isSpeedBoostEnabled = false;

    private bool _isShieldEnabled = false;

    private SpawnManager _spawnManager;

    private GameManager _gameManager;

    private UIManager _uiManager;

    private float _speedMultiplier = 2;

    private float _canFire = -1.0f;

    public bool playerOne = false;

    public bool playerTwo = false;

    private Animator _animator;

    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager.isTwoPlayer == false)
        {
            transform.position = new Vector3(0, -1.9f, 0);
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); //to access spawn manager script
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _LaserAudioSource = GetComponent<AudioSource>();
        _ExplosionAudioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if(_LaserAudioSource == null)
        {
            Debug.LogError("Audio source on player is NULL");
        }
       else
        {
            _LaserAudioSource.clip = _LaserAudioClip;
        }

        if(_ExplosionAudioSource == null)
        {
            Debug.LogError("Explosion Audio Source is NULL");
        }
        else
        {
            _ExplosionAudioSource.clip = _ExplosionClip;
        }

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
    }


    void Update()
    {
        CalculateMovementPlayerOne();
        CalculateMovementPlayerTwo();

        if((Input.GetKeyUp(KeyCode.Space) && Time.time > _canFire && playerOne) || Input.GetKeyUp(KeyCode.KeypadEnter) && Time.time > _canFire && playerTwo)
        {
            FireLaser();
        }

#if UNITY_ANDROID
        else if(CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#endif
    }

    void CalculateMovementPlayerOne()
    {
        if(playerOne)
        {
            float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
            float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");   //Input.GetAxis("Vertical");
            transform.Translate(_speed * horizontalInput * Time.deltaTime * Vector3.right);
            transform.Translate(_speed * verticalInput * Time.deltaTime * Vector3.up); //player moves at real time * speed using keys found under axis options in unity

            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
            }
            else if (transform.position.y <= -3.8f)
            {
                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }

            if (transform.position.x >= 11.3f)
            {
                transform.position = new Vector3(-11.3f, transform.position.y, 0);
            }
            else if (transform.position.x <= -11.3f)
            {
                transform.position = new Vector3(11.3f, transform.position.y, 0);
            }
        }
    }

    void CalculateMovementPlayerTwo()
    {
        if(playerTwo)
        {
        float horizontalInput = CrossPlatformInputManager.GetAxis("HorizontalTwo");
        float verticalInput = CrossPlatformInputManager.GetAxis("VerticalTwo");
        transform.Translate(_speed * horizontalInput * Time.deltaTime * Vector3.right);
        transform.Translate(_speed * verticalInput * Time.deltaTime * Vector3.up);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        }

    }

   void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if(_isTripleShotEnabled == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); //spawns the laser 1.05 above player
        }
        _LaserAudioSource.clip = _LaserAudioClip;
        _LaserAudioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldEnabled == true)
        {
            _isShieldEnabled = false;
            _shieldVisualiser.SetActive(false);
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

        if (_lives < 1)
        {
            _uiManager.CheckForBestScore();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


    public void TripleShotActive()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotEnabled = false;

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostEnabled = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostEnabled = false;
        _speed /= _speedMultiplier;
    } 

    public void ShieldActive()
    {
        _isShieldEnabled = true;
        _shieldVisualiser.SetActive(true);
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
