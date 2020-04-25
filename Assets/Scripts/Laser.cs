using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 10f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Destroy 'lasers' when out of screen.
        if(transform.position.y > 8.0f || transform.position.y < -8.0f) // Here we handle bouth our and the Enemy lasers depending on if they are + or -
        {
            if(transform.parent != null)
            {
                Transform parent = transform.parent;
                Destroy(parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
