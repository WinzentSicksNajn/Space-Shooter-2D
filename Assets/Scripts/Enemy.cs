using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private Player _player;
    private Animator _animator;
    private AudioSource _explosionAudio;
    private float _waitAndShoot;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        { Debug.LogError("Player object is null!"); }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        { Debug.LogError("Animator object is null!"); }

        _explosionAudio = GetComponent<AudioSource>();
        if (_explosionAudio == null)
        { Debug.LogError("AudioSource is null!"); }

        _waitAndShoot = Random.Range(5.0f, 7.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If Enemy gets out of bound, we respawn it at a new random position.
        if(transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 8.0f, 0f);
        }

        _waitAndShoot -= Time.deltaTime;
        if (_waitAndShoot < 0)
        { Shoot(); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.transform.name);
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
                player.AddToScore(100);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddToScore(100);
            }
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    void Shoot()
    {
        Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0,-0.8f,0), Quaternion.identity);
        _waitAndShoot = Random.Range(5.0f, 7.0f);
    }
}
