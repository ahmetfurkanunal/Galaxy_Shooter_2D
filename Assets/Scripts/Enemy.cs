using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
   
    private Animator _animator;
    private Player1 _player1;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    // Start is called before the first frame update
    void Start()
    {
       _player1 = GameObject.Find("Player1").GetComponent<Player1>();
        _audioSource =GetComponent<AudioSource>();  

        if(_player1 != null)
        {
            Debug.LogError("The player is null.");
        }    
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            Debug.LogError("Animator is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate= Random.Range(-3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser1[] lasers1 = enemyLaser.GetComponentsInChildren<Laser1>();   

            for(int i=0; i<lasers1.Length; i++)
            {
                lasers1[i].AssignEnemyLaser();
            }
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if other is Player
 
 
        //Destroy us
        if (other.tag == "Player1")
        {
            //damage the player
            Player1 player1 = other.transform.GetComponent<Player1>();
            
            if (player1 != null) 
            {
                player1.Damage();       
            }
            //trigger animation
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);   
            
        }
        //if other is laser
        //laser
        //destroy us 
        if (other.tag == "Laser1")
        {
            Destroy(other.gameObject, 2.8f);
           
            if ( _player1 != null)
            {
                _player1.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
           
        }
    }
   
}
