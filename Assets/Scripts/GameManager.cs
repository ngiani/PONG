using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();

            return _instance;
        }
    }

    [SerializeField] Ball ball;

    private int _round;

    // Start is called before the first frame update
    void Start()
    {
        //Launch the ball first time when game starts
        LaunchBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LaunchBall()
    {
        Vector2 _launchDirection;

        //Launch ball on the left on even rounds
        if (_round % 2 == 0)
        {
            _launchDirection = new Vector2(-Random.Range(0.4f, 1.0f), Random.Range(0.0f, 1.0f));

          
        }

        //Launch ball on the right on odd rounds
        else
        {

            _launchDirection = new Vector2(Random.Range(0.4f, 1.0f), Random.Range(0.0f, 1.0f));
        }

        ball.GetComponent<Rigidbody2D>().AddForce(_launchDirection.normalized * ball.Speed, ForceMode2D.Impulse);
    }

}
