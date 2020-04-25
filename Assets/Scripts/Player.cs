using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _trippleShotPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _leftFire;
    [SerializeField] private GameObject _rightFire;
    [SerializeField] private float _speed = 5.5f;
    [SerializeField] private float _speedSpeedUp = 8.5f;
    [SerializeField] private float _fireDelay = 0.15f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private AudioClip[] _audioClips;
    private float _nextFire = 0f;
    private SpawnManager _spawnManager;
    private bool _trippleShot = false;
    private float _powerUpTrippleShotActiveTimer = 5.0f;
    private bool _speedUp = false;
    private float _powerUpSpeedUpActiveTimer = 8.0f;
    private bool _shieldActive = false;
    private GameObject _shieldObject;
    private int _score = 0;
    private UI_Manager _uiManager;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // Get a refrenence to the SpawnManager script.
        if(_spawnManager == null)
        { Debug.LogError("SpawnManager is null!"); }
        _uiManager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();
        if(_uiManager == null)
        { Debug.LogError("UI_Manager is null!"); }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        { Debug.LogError("AudioSource is null!"); }

        _uiManager.UpdateLives(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireWeapons();
    }

    // Handle Player movement.
    void CalculateMovement()
    {
        // Get player inputs.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Move player ship.
        if (_speedUp)
        {
            transform.Translate(Vector3.right * horizontalInput * _speedSpeedUp * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speedSpeedUp * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }

        // Restrict our vertical movment.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), transform.position.z); // Simpler solution to do the same thing.
        /*if (transform.position.y > 0f)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);
        }*/

        // Restrict our horizontal movment. (Adding wrap-around.)
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
    }

    // Fire our weapons.
    void FireWeapons()
    {
        // Fire weapons if we can and should.
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && (Time.time > _nextFire))
        {
            _nextFire = Time.time + _fireDelay;
            if(_trippleShot)
            {
                Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
            }
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();
        }
    }

    // When a enemy damges us.
    public void Damage()
    {
        // If we have a shield, remove it and don't do anything more.
        if(_shieldActive)
        {
            if (_shieldObject != null)
            {
                Destroy(_shieldObject);
            }
            _shieldActive = false;
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);
        if (_lives == 2)
        {
            _leftFire.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightFire.SetActive(true);
        }
        else if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.ShowGameOver(true);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    // When we pick up a TrippleShot PowerUp.
    public void PowerUpTrippleShot()
    {
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
        _trippleShot = true;
        StartCoroutine("DisableTrippleShot");
    }

    // Starta a timer to diable our trippleshot.
    IEnumerator DisableTrippleShot()
    {
        while (true)
        {
            yield return new WaitForSeconds(_powerUpTrippleShotActiveTimer); // Wait n seconds and then continue executing the code.
            _trippleShot = false;
        }
    }

    // When we pick up a Speed PowerUp.
    public void PowerUpSpeed()
    {
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
        _speedUp = true;
        StartCoroutine("DisableSpeedUp");
    }

    // Starta a timer to diable our trippleshot.
    IEnumerator DisableSpeedUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(_powerUpSpeedUpActiveTimer); // Wait n seconds and then continue executing the code.
            _speedUp = false;
        }
    }

    // When we pick up a Shield PowerUp.
    public void PowerUpShield()
    {
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
        if (_shieldActive == false) // So we dont add more visual shields even though we only can take one hit and only remove one visual shield. :p
        {
            _shieldObject = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
            _shieldObject.transform.parent = this.transform;
        }
        else
        { AddToScore(1000); } // If we pick up an other shield when we have one, we get extra points to our score.
        _shieldActive = true;
    }

    // Add points we rescieve from killing enemies and tell the UI to update our score.
    public void AddToScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    // Check to se if we get hit by Enemy lasers!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyLaser")
        {
            GameObject parent = collision.transform.parent.gameObject; // So we destroy the other laser as well as the container for them.
            Destroy(parent);
            Damage();
        }
    }
}
