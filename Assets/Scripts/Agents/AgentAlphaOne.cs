using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using MineSweeper;
using System;
using Sirenix.OdinInspector;
using Unity.MLAgents.Policies;

public class AgentAlphaOne : Agent
{
    public Action<float> OnRewardUpdated;

    [SerializeField]
    private Game game;

    [SerializeField]
    private float lossReward = -50f;

    [SerializeField]
    private float winReward = 50f;

    [SerializeField]
    private float requestDecisionTime = 100f;

    [SerializeField]
    private bool useCustomRequestDecision = false;

    protected override void OnEnable() {
        base.OnEnable();

        game.OnScoreUpdate += OnScoreUpdated;
        game.OnGameWon += OnGameWon;
        game.OnGameLost += OnGameLost;

        if (useCustomRequestDecision) {
            // GetComponent<DecisionRequester>().enabled = false;
            MaxStep = BoardManager.Instance.Height * BoardManager.Instance.Width * 10;
            InvokeRepeating("RequestDecision", requestDecisionTime, requestDecisionTime);
        }
    }

    protected override void OnDisable() {
        base.OnDisable();

        game.OnScoreUpdate -= OnScoreUpdated;
        game.OnGameWon -= OnGameWon;
        game.OnGameLost -= OnGameLost;
    }

    private void OnGameLost()
    {
        AddReward(lossReward);
        EndEpisode();
    }

    private void OnGameWon()
    {
        AddReward(winReward);
        EndEpisode();
    }

    private void OnScoreUpdated(float reward)
    {
        // Debug.Log("Score updated: " + reward);
            
        AddReward(reward);
    }

    public override void OnEpisodeBegin()
    {
        GameManager.Instance.GameFinished();
        game.NewGame();
    }

    public override void AddReward(float reward) {
        base.AddReward(reward);

        OnRewardUpdated?.Invoke(GetCumulativeReward());
    }

    public override void SetReward(float reward)
    {
        base.SetReward(reward);

        OnRewardUpdated?.Invoke(GetCumulativeReward());
    }

    private float GetReward() {
        return GetCumulativeReward();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(game.informationUtilities.relevantTilesFloats);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        int moveX = actions.DiscreteActions[0];
        int moveY = actions.DiscreteActions[1];
        // int moveType = actions.DiscreteActions[2];

        game.Move(moveX, moveY, 0);
    }
}
