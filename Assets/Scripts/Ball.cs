using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] float _speed;

    Rigidbody2D _rigidBody;

    Vector2 _launchDirection;
    Vector2 _velocityBeforeCollision;

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

    void FixedUpdate()
    {
        //Avoid loss of velocity over time by multiplying to a constant speed value
        _rigidBody.velocity = _rigidBody.velocity.normalized * _speed;

        //Save velocity to use it inside OnCollisionEnter, before it is modified by the physics engine
        _velocityBeforeCollision = _rigidBody.velocity;
    }

    public void Launch()
    {
        //Reset velocity before launch
        _rigidBody.velocity = Vector2.zero;

        //Launch ball to the left side on even rounds
        if (GameManager.Instance.Round % 2 == 0)
        {
            _launchDirection = new Vector2(-Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f));

        }

        //Launch ball to the right side on odd rounds
        else
        {

            _launchDirection = new Vector2(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f));
        }

        _rigidBody.AddForce(_launchDirection.normalized * _speed, ForceMode2D.Impulse);
    }

    //Forecast collision point of the ball with the given collider
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
        //Play sound on collision 
        GetComponent<AudioSource>().Play();


        //Reflect ball on collision adding a random vector, to avoid ball bouncing forever along the same direction
        float randAmount = 0.5f;
        Vector2 randomVector = new Vector2(Random.Range(-randAmount, randAmount), Random.Range(-randAmount, randAmount));

        Vector2 inDirection = _velocityBeforeCollision;
        Vector2 inNormal = collision.contacts[0].normal;

        Vector2 outDirection = Vector2.Reflect(inDirection, inNormal) + randomVector;

        //Avoid reflected velocity with zero components : reflection angle should always be large enough
        _rigidBody.velocity = RemoveCloseToZeroComponents(outDirection);
    }


    Vector2 RemoveCloseToZeroComponents(Vector2 vector)
    {
        Vector2 outVector = vector;

        float threshold = 4;

        if (vector.x >= -threshold && vector.x <= 0)
            outVector.x = -threshold;

        else if (vector.x <= threshold && vector.x >= 0)
            outVector.x = threshold;

        if (vector.y >= -threshold && vector.y <= 0)
            outVector.y = -threshold;

        else if (vector.y <= threshold && vector.y >= 0)
            outVector.y = threshold;

        return outVector;
    }
}
