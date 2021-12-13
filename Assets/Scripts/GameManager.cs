using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static GoalCollider;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }

    private int _round;

    public int Round => _round;

    Racket leftRacket;
    Racket rightRacket;

    Vector3 startingLeftRacketPos;
    Vector3 startingRightRacketPos;

    private int _leftScore = 0;
    private int _rightScore = 0;

    GoalCollider leftGoalCollider;
    GoalCollider rightGoalCollider;

    [SerializeField] AudioClip _victory;
    [SerializeField] AudioClip _defeat;


    public class ScoreEvent : UnityEvent<SIDE, int>
    {

    }

    public ScoreEvent ScoreAssigned = new ScoreEvent();
    public SideEvent SideHasWon = new SideEvent();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnScoredGoal(SIDE side)
    {
        //Assign score to the the player who let in a goal
        if (side == SIDE.LEFT)
        {
            _rightScore++;

            ScoreAssigned?.Invoke(SIDE.RIGHT, _rightScore);
        }

        else if (side == SIDE.RIGHT)
        {
            _leftScore++;

            ScoreAssigned?.Invoke(SIDE.LEFT, _leftScore);
        }

        //Game ends when one player reaches 11 
        if (_leftScore == 11)
        {
            SideHasWon?.Invoke(SIDE.LEFT);

            //Play victory sound if left is player, otherwise defeat sound
            if (leftRacket.IsPlayer)
            {
                GetComponent<AudioSource>().clip = _victory;
                GetComponent<AudioSource>().Play();
            }

            else
            {
                GetComponent<AudioSource>().clip = _defeat;
                GetComponent<AudioSource>().Play();
            }
              

            StartCoroutine(WaitAndGoBackToMenu());
        }

        else if (_rightScore == 11)
        {
            SideHasWon?.Invoke(SIDE.RIGHT);

            //Play victory sound if right is player, otherwise defeat sound
            if (rightRacket.IsPlayer)
            {
                GetComponent<AudioSource>().clip = _victory;
                GetComponent<AudioSource>().Play();
            }

            else
            {
                GetComponent<AudioSource>().clip = _defeat;
                GetComponent<AudioSource>().Play();
            }

            StartCoroutine(WaitAndGoBackToMenu());
        }

        //Otherwise start a new round
        else
        {
            StartCoroutine(WaitAndStartNewRound());
        }

    }

    //Start new round after waiting some seconds and reset the ball on starting position, then launch it
    IEnumerator WaitAndStartNewRound()
    {
        yield return new WaitForSeconds(2.0f);

        _round++;

        leftRacket.transform.position = startingLeftRacketPos;
        rightRacket.transform.position = startingRightRacketPos;

        Ball.Instance.transform.position = Vector2.zero;
        Ball.Instance.Launch();
    }

    IEnumerator WaitAndGoBackToMenu()
    {
        yield return new WaitForSeconds(2.0f);

        LoadMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "main")
        {
            //Reset score and rounds
            _leftScore = 0;
            _rightScore = 0;
            _round = 0;

            //Launch the ball first time when game starts
            Ball.Instance.Launch();

            leftRacket = GameObject.Find("leftRacket").GetComponent<Racket>();
            rightRacket = GameObject.Find("rightRacket").GetComponent<Racket>();

            startingLeftRacketPos = leftRacket.transform.position;
            startingRightRacketPos = rightRacket.transform.position;

            leftGoalCollider = GameObject.Find("leftGoalCollider").GetComponent<GoalCollider>();
            rightGoalCollider = GameObject.Find("rightGoalCollider").GetComponent<GoalCollider>();

            leftGoalCollider.ScoredGoal.AddListener(OnScoredGoal);
            rightGoalCollider.ScoredGoal.AddListener(OnScoredGoal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetFloor()
    {
        return GameObject.Find("floor");
    }

    public GameObject GetRoof()
    {
        return GameObject.Find("roof"); ;
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene("main");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
