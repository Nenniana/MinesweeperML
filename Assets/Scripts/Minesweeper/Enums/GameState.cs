using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper
{
    public enum GameState
    {
        InProgress = 10,
        GameOver = 20,
        Won = 30,
        Calculating = 40,
        Visualizing = 50
    }
}