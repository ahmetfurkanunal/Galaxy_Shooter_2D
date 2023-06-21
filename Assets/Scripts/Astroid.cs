using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 25.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;

    

    private void Start()
    {
            _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();  
    }

    void Update()
    {
        // rotate object on the zed axis 
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    //check for LASER collision (Trigger)
    //instantiate explosion at the position of the asteroid (us)
    //destroy the explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser1")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();  
            Destroy(this.gameObject, 0.25f);
        }
    }
}
