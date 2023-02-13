using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace MineSweeper {
    public class MoveUtilities
    {
        [ShowInInspector]
        internal List<Move> moves = new List<Move>();

        [SerializeField]
        private List<List<Move>> previousGameMoves;

        internal int currentMoveIndex = -1;

        internal void SaveMove(MoveType moveType, Vector2Int gridPosition, Tile[,] state)
        {
            moves.Add(new Move(moveType, gridPosition, state.Clone() as Tile[,]));
            currentMoveIndex++;
        }

        internal Move GetMove(int index) {
            if (moves != null && moves.Count >= index)
                return moves[index];
                
            return null;
        }

        internal Move GetCurrentMove() {
            return GetMove(currentMoveIndex);
        }

        internal Move GetPreviousMove()
        {
            return GetMove(currentMoveIndex - 1);
        }

        internal void UnloadCurrentMoves() {
            if (moves.Count > 0) {
                if (previousGameMoves == null)
                    previousGameMoves = new List<List<Move>>();

                previousGameMoves.Add(moves);
                currentMoveIndex = -1;
                moves = new List<Move>();
            }
        }
    }
}