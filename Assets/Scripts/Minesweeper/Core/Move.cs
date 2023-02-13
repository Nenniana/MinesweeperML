using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MineSweeper
{
    [InlineEditor(Expanded = true)]
    public class Move
    {
        internal Move(MoveType _moveType, Vector2Int _gridPosition, Tile[,] _state) {
            this.moveType = _moveType;
            this.gridPosition = _gridPosition;
            this.state = _state;
        }

        [ShowInInspector]
        internal Vector2Int gridPosition;
        [ShowInInspector]
        internal MoveType moveType;
        // TODO: Write custom TableMatrix to show state in inspector.
        // [ShowInInspector]
        internal Tile[,] state;
    }
}