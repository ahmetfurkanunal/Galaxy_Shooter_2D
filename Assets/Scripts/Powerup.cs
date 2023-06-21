using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    // Start is called before the first frame update
    
    [SerializeField]
    private int powerupID; //0 = Triple Shot 1=Speed 2=Shields
    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3 ()
        //when the leave screen,destroy this object
        transform.Translate(Vector3.down* _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }
    //OnTriggerCollision
    //Only be collectable by the Player (HINT: Use Tags)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player1")
        {
            //communicate with the player script
            // handle to the component i want
            //assign the handle to the component
            Player1 player1 =other.transform.GetComponent<Player1>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if( player1 != null)
            {
                        
                switch(powerupID)
                {
                    case 0:
                        player1.TripleShotActive();
                        break;
                    case 1:
                        player1.SpeedBoostActive(); 
                        break;
                    case 2:
                        player1.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
