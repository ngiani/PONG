using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] float Speed;

    Rigidbody2D _rigidBody;
    Vector2 _launchDirection;

    static Ball _instance;
    public static Ball Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Ball>();

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void Launch()
    {
        //Launch ball on the left on even rounds
        if (GameManager.Instance.Round % 2 == 0)
        {
            _launchDirection = new Vector2(-Random.Range(0.4f, 1.0f), Random.Range(0.0f, 1.0f));


        }

        //Launch ball on the right on odd rounds
        else
        {

            _launchDirection = new Vector2(Random.Range(0.4f, 1.0f), Random.Range(0.0f, 1.0f));
        }

        _rigidBody.AddForce(_launchDirection.normalized * Speed, ForceMode2D.Impulse);
    }

    //Forecast interception point of the ball with the given collider
    public bool GetCollisionPointWith(string[] layers, out Vector2 collisionPoint)
    {

        RaycastHit2D result = Physics2D.Raycast(transform.position, _rigidBody.velocity, Mathf.Infinity, LayerMask.GetMask(layers));


        collisionPoint = result.point;

        return result.collider != null;
    }

    public Vector3 GetVelocity()
    {
        return _rigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidBody.AddForce(-collision.contacts[0].normal + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

}
