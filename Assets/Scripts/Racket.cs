using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    [SerializeField] bool _player;
    [SerializeField] float _speed;

    public bool IsPlayer => _player;

    private Racket enemyRacket;


    private GameObject floor;
    private GameObject roof;

    private BoxCollider2D floorCollider;
    private CapsuleCollider2D racketCollider;
    private BoxCollider2D roofCollider;

    Vector2 _predictedCollisionPoint;

    private float AIReactionTime;
    bool isAIWaitingToGetNextPoint;
    float elapsedAIWaitingTime;


    // Start is called before the first frame update
    void Start()
    {
        floor = GameManager.Instance.GetFloor();
        roof = GameManager.Instance.GetRoof();

        enemyRacket = new List<Racket>(FindObjectsOfType<Racket>()).Find(r => !r.Equals(this));

        floorCollider = floor.GetComponent<BoxCollider2D>();
        racketCollider = GetComponent<CapsuleCollider2D>();
        roofCollider = roof.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player)
        {
            MoveWithUserInput();
        }

        else
        {
            AutoMove(_speed);
        }
    }

    void MoveWithUserInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !IsCollidingWithRoof())
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && !IsCollidingWithFloor())
        {
            transform.Translate(-Vector3.up * _speed * Time.deltaTime);
        }
    }

    #region AI
    public void AutoMove(float speed)
    {
        float offset = 0.1f;

        //Check if ball is colliding with racket's score collider, and move towards predicted collision point after waiting a random reaction time to simulate human slow reaction time to the incoming ball
        if (BallIsComing() && BallIsNear())
        {
            if (isAIWaitingToGetNextPoint)
            {
                elapsedAIWaitingTime += Time.deltaTime;

                if (elapsedAIWaitingTime >= AIReactionTime)
                {
                    elapsedAIWaitingTime = 0.0f;
                    isAIWaitingToGetNextPoint = false;
                }
            }

            else
            {
                Ball.Instance.GetCollisionPointWith(new string[] {"ScoreCollider", "Surface" }, out _predictedCollisionPoint);

                AIReactionTime = Random.Range(0.1f, 0.3f);

                isAIWaitingToGetNextPoint = true;
            }

            MoveTowardsCollisionPoint(speed, offset);
        }

        //If it's not going to collide, move back towards center
        else
        {
            //MoveTowardsCenter(speed, offset);

            elapsedAIWaitingTime = 0.0f;
            isAIWaitingToGetNextPoint = false;
        }
    }

    void MoveTowardsCollisionPoint(float speed, float distanceOffset)
    {
        if (_predictedCollisionPoint.y > (transform.position.y + distanceOffset) && !IsCollidingWithRoof())
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        else if (_predictedCollisionPoint.y < (transform.position.y - distanceOffset) && !IsCollidingWithFloor())
        {
            transform.Translate(-Vector3.up * _speed * Time.deltaTime);
        }
    }

    void MoveTowardsCenter(float speed, float centerOffset)
    {
        if (transform.position.y > centerOffset)
        {
            transform.Translate(-Vector3.up * _speed * Time.deltaTime);
        }

        else if (transform.position.y < -centerOffset)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }

    bool BallIsComing()
    {
        return Ball.Instance.GetVelocity().x > 0 && transform.position.x > 0 || Ball.Instance.GetVelocity().x < 0 && transform.position.x < 0;
    }

    bool BallIsNear()
    {
        return Vector3.Distance(Ball.Instance.transform.position, transform.position) <= (Vector3.Distance(transform.position, enemyRacket.transform.position) / 2) + Random.Range(-2.5f, 2.5f);
    }

    #endregion

    #region COLLISIONS
    bool IsCollidingWithFloor()
    {
        float floorHeight = floor.transform.position.y + floorCollider.size.y / 2;
        float racketHeight = transform.position.y - racketCollider.size.y / 2;

        return racketHeight <= floorHeight;
    }

    bool IsCollidingWithRoof()
    {
        float roofHeight = roof.transform.position.y - roofCollider.size.y / 2;
        float racketHeight = transform.position.y + racketCollider.size.y / 2;

        return racketHeight >= roofHeight;
    }
    #endregion

}
