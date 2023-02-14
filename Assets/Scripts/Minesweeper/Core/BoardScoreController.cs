using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScoreController : MonoBehaviour
{
    [SerializeField]
    private int r, b, g;
    [SerializeField]
    private AgentAlphaOne agentAlphaOne;

    private TMPro.TextMeshProUGUI text;

    private void Awake() {
        TryGetComponent<TMPro.TextMeshProUGUI>(out text);
    }

    private void OnEnable() {
        if (GameManager.Instance.EnableScores)
            agentAlphaOne.OnRewardUpdated += OnRewardUpdated;
        else
            gameObject.SetActive(false);
    }

    private void OnDisable() {
        if (GameManager.Instance.EnableScores)
            agentAlphaOne.OnRewardUpdated -= OnRewardUpdated;
    }

    private void OnRewardUpdated(float score)
    {
        text.text = "Score: " + score.ToString("0.0");
        
        if (score >= 0)
            text.color = Color.white;
        else
            text.color = Color.black;
    }
}
