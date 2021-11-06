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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ScoreAssigned.AddListener(OnScoreAssigned);
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
