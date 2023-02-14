using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInformationController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI overallWinRatioText;
    [SerializeField]
    private TMPro.TextMeshProUGUI currentWinRatioText;
    [SerializeField]
    private TMPro.TextMeshProUGUI totalGamesPlayedText;
    [SerializeField]
    private TMPro.TextMeshProUGUI gamesPerIntervalText;

    private void OnEnable() {
        GameManager.Instance.OnIntervalOver += UpdateInformation;
    }

    private void OnDisable() {
        GameManager.Instance.OnIntervalOver -= UpdateInformation;
    }

    private void UpdateInformation()
    {
        gamesPerIntervalText.text = CalculateGamesPerInterval();
        overallWinRatioText.text = CalculateWinRatio(GameManager.Instance.GamesWon, GameManager.Instance.GamesFinishedInTotal, "OWR: ");
        currentWinRatioText.text = CalculateWinRatio(GameManager.Instance.GamesWonInInterval, GameManager.Instance.GamesFinishedInInterval, "CWR: ");
        totalGamesPlayedText.text = CalculateTotalGamesPlayed();
    }

    private string CalculateTotalGamesPlayed()
    {
        return "TG: " + GameManager.Instance.GamesPlayedInTotal;
    }

    private string CalculateWinRatio(int gamesWon, int gamesPlayed, string _text)
    {
        string text = _text;

        if (gamesWon > 0) {
            if (gamesPlayed > 0)
                return text += (((float)gamesWon / (float)gamesPlayed) * 100.0f).ToString("0.0") + "%";
            return text + "0%";
        }

        return text;
    }

    private string CalculateGamesPerInterval()
    {
        return "G/S: " + GameManager.Instance.GamesFinishedInInterval;
    }
}