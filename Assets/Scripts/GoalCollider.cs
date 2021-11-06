using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalCollider : MonoBehaviour
{
    public enum SIDE {LEFT, RIGHT}

    [SerializeField] SIDE _side;

    public class SideEvent : UnityEvent<SIDE>
    {

    }

    public SideEvent ScoredGoal = new SideEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("SOMETHING ENTERED");

        if (other.tag == "ball")
        {
            Debug.Log("BALL ENTERED");

            ScoredGoal?.Invoke(_side);
        }
    }

}
