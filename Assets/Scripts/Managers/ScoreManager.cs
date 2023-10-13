using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    [SerializeField]
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            HandleScore();
        }
    }

    [Header("Level Start Scores")]
    public int thresholdBronze;
    public int thresholdSilver;
    public int thresholdGold;

   

    public int ballsLeft = 3;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }

    private void Start()
    {
        CalculateThresholds();
        UIManager.instance.UpdateStarsThreshHolds();

    }

    private void CalculateThresholds()
    {
        Plank[] planks = FindObjectsOfType<Plank>();
        //enemies
        thresholdGold = 0;

        for (int i = 0; i < planks.Length; i++)
        {
            thresholdGold += planks[i].points;



        }
        thresholdGold = Mathf.RoundToInt(thresholdGold * 0.9f);
        thresholdSilver = Mathf.RoundToInt(thresholdGold * 0.75f);

        thresholdBronze = Mathf.RoundToInt(thresholdSilver * 0.75f);

    }

    public static void IncreasePoint(int points) {
        instance.Score += points;
    }
    private void HandleScore()
    {
        UIManager.instance.UpdateStarsThreshHolds();
    }
    public bool isAboveThresholds => score >= thresholdBronze;

}
