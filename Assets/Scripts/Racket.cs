using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    [SerializeField] bool _player;
    [SerializeField] float _speed;

    Rigidbody2D _rigidBody;
    GameObject _collidedObject;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

      
    }

    private void FixedUpdate()
    {
        if (_player)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !(_collidedObject?.name == "roof"))
            {
                _rigidBody.velocity = new Vector2(0, 1) * _speed;
            }

            else if (Input.GetKey(KeyCode.DownArrow) && !(_collidedObject?.name == "floor"))
            {
                _rigidBody.velocity = new Vector2(0, -1) * _speed;
            }

            else
            {
                _rigidBody.velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collidedObject = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _collidedObject = null;
    }
}
