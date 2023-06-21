using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player1 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;    
    private bool _isTripleShotActive = false;
    private bool _IsSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private SpawnManager _spawnManager;
    //variable reference to the shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;


    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>(); 
        _audioSource=GetComponent<AudioSource>();   

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        if(_uiManager == null) {
            Debug.LogError("The UI Manager is null");            
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // new Vector3(-5, 0, 0) * 5 * real time


        //transform.Translate(Vector3.left * horizontalInput *  _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        // transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);


        //if y position greater than 0
        // y=0
        //if y position less than -3.8f
        //y stay there -3.8f

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //if x position greater than 11
        // x=-11
        //if y position less than -11.3ff
        //x=11
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + fireRate;

        if (_isTripleShotActive == true)
        {
            //instantiate for the triple shot
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();    
    }

    public void Damage()
    {
        //if shield is active
        //do nothing
        //deactivate shields 
        //return;

        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if(_lives == 2)
        {
            _leftEngine.SetActive(true); 
        } else if(_lives == 1){
            _rightEngine.SetActive(true);   
        }

        if (_lives < 1)
        {
            //Communicate with Spawn Manager
            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);

        }
    }

    public void TripleShotActive()
    {
        //tripleShotActive becomes true 
        _isTripleShotActive = true;
        //start  the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //wait 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {

        _IsSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    //methods to add 10 to score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}