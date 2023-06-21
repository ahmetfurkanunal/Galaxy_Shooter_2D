using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser1 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        //translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //id laser position > 8
        //destroy the object

        if (transform.position.y > 8f)
        {
            //check if this object has a parent
            //destroy the parent too!
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);

        }
    }
    void MoveDown()
    {
        //translate laser down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //id laser position < - 8
        //destroy the object
        if (transform.position.y < -8f)
        {
            //check if this object has a parent
            //destroy the parent too!
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player1" && _isEnemyLaser ==true)
        {
            Player1 player1 = other.GetComponent<Player1>();   
            if(player1 != null)
            {
                player1.Damage();
            }
        }
        
    }
}