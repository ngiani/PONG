using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            return _instance;
        }
    }

    [SerializeField] TextMeshProUGUI _leftScoreLabel;
    [SerializeField] TextMeshProUGUI _rightScoreLabel;
    [SerializeField] TextMeshProUGUI _victoryLabel;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ScoreAssigned.AddListener(OnScoreAssigned);
        GameManager.Instance.SideHasWon.AddListener(OnSideVictory);
    }

    private void OnSideVictory(GoalCollider.SIDE side)
    {
        switch (side)
        {
            case GoalCollider.SIDE.LEFT:
                _victoryLabel.text = "LEFT PLAYER WON! ";
                break;
            case GoalCollider.SIDE.RIGHT:
                _victoryLabel.text = "RIGHT PLAYER WON! ";
                break;
        }
    }

    private void OnScoreAssigned(GoalCollider.SIDE side, int score)
    {
        switch (side)
        {
            case GoalCollider.SIDE.LEFT:
                _leftScoreLabel.text = score.ToString();
                break;
            case GoalCollider.SIDE.RIGHT:
                _rightScoreLabel.text = score.ToString();
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
