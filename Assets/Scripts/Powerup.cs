using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int powerupID; //0 = triple shot, 1 = speed boost, 2 = shields up

    [SerializeField]
    private AudioClip _clip;



    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if(transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                         player.TripleShotActive();
                         break;
                    case 1:
                        player.SpeedBoostActive();
                         break;
                    case 2:
                         player.ShieldActive();
                         break;
                    default:
                         break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
