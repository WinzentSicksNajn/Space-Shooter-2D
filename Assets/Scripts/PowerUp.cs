using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private int _powerUpID = 0; // 0 = TrippleShot, 1 = Speed, 2 = Shield
    //[SerializeField] private AudioClip _powerUpAudio;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If PowerUp is out of bounds, destroy it.
        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        player.PowerUpTrippleShot();
                        break;
                    case 1:
                        player.PowerUpSpeed();
                        break;
                    case 2:
                        player.PowerUpShield();
                        break;
                    default:
                        Debug.Log("Error: Unknown PowerUp ID:" + _powerUpID);
                        break;
                }
            }
            //AudioSource.PlayClipAtPoint(_powerUpAudio, transform.position); // A solution to play audioclips at a position, even if we destroy the initiator of the audio.
            Destroy(this.gameObject);
        }
    }
}
