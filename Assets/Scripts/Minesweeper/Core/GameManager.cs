using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action OnGameWon, OnGameLost, OnGameOver, OnIntervalOver;

    [HideInInspector]
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private float intervalWaitTime = 60f;

    [SerializeField]
    private bool enableScores = true;

    public int GamesWon { get => gamesWon; private set => gamesWon = value; }
    public int GamesLost { get => gamesLost; private set => gamesLost = value; }

    // All games that were won or lost.
    public int GamesFinishedInTotal { get => gamesFinishedInTotal; private set => gamesFinishedInTotal = value; }

    // All games during interval that were won or lost.
    public int GamesFinishedInInterval { get => gamesFinishedInInterval; private set => gamesFinishedInInterval = value; }
    public float IntervalWaitTime { get => intervalWaitTime; private set => intervalWaitTime = value; }
    public int GamesWonInInterval { get => gamesWonInInterval; private set => gamesWonInInterval = value; }

    // All games played, even those that were never won/lost.
    public int GamesPlayedInTotal { get => gamesPlayedInTotal; private set => gamesPlayedInTotal = value; }
    public bool EnableScores { get => enableScores; private set => enableScores = value; }
    public int GamesPlayedInInterval { get => gamesPlayedInInterval; private set => gamesPlayedInInterval = value; }

    private int gamesWon, gamesLost, gamesFinishedInTotal, gamesFinishedInInterval, gamesWonInInterval, gamesPlayedInTotal, gamesPlayedInInterval = 0;
    private float intervalTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GameWonLost(bool won) {
        GamesFinishedInTotal++;
        GamesFinishedInInterval++;
        GamesPlayedInInterval++;

        OnGameOver?.Invoke();

        if (won)
        {
            OnGameWon?.Invoke();
            GamesWon++;
            GamesWonInInterval++;
        } else {
            OnGameLost?.Invoke();
            GamesLost++;
        }
    }

    public void GameFinished() {
        GamesPlayedInTotal++;

        OnGameOver?.Invoke();
    }

    private void Update() {
        intervalTimer += Time.deltaTime;

        if (intervalTimer >= IntervalWaitTime)
        {
            OnIntervalFinished();
            intervalTimer = 0f;
        }
    }

    private void OnIntervalFinished() {
        OnIntervalOver?.Invoke();
        GamesFinishedInInterval = 0;
        GamesPlayedInInterval = 0;
        GamesWonInInterval = 0;
    }
}
