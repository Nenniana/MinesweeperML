using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper 
{
    public enum TileState
    {
        Unrevealed = 0,
        Revealed = 10,
        Flagged = 20,
        Exploded = 30
    }
}