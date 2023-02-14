using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MineSweeper
{
    public class InformationUtilities
    {
        [ShowInInspector]
        internal List<Tile> possibleMoves;

        [ShowInInspector]
        internal List<Tile> relevantTiles;

        internal List<float> relevantTilesFloats;

        private Game game;

        public void Initialize(Tile[,] state, Game _game)
        {
            game = _game;
            // possibleMoves = new List<Tile>();
            // relevantTiles = new List<Tile>();
            relevantTilesFloats = new List<float>();

            // possibleMoves = state.OfType<Tile>().ToList();
            // relevantTiles = possibleMoves;
            GenerateFloats(state);
        }

        private void GenerateFloats(Tile[,] state)
        {
            relevantTilesFloats.Clear();

            for (int x = 0; x < state.GetLength(0); x++)
            {
                for (int y = 0; y < state.GetLength(1); y++)
                {
                    float enumValue = 0f;

                    if (state[x, y].State == TileState.Unrevealed)
                        enumValue = (float)TileType.Unrevealed;
                    else
                        enumValue = (float)state[x, y].Type;

                    relevantTilesFloats.Add(enumValue);
                }
            }
        }

        private string LogTileFloats(Tile[,] state)
        {
            string toReturn = "";
            toReturn += "\t0\t1\t2\t3\t4\t5\t6\t7\t8\t9\t10\t11\t12\t13\t14\t15\n\n";
            for (int x = state.GetLength(0) - 1; x > -1; x--)
            {
                toReturn += (state.GetLength(0) - x - 1).ToString() + "\t";
                for (int y = 0; y < state.GetLength(1); y++)
                {
                    int index = (x % state.GetLength(0)) + (y * state.GetLength(1));
                    string firstSub = ((TileType)((int)relevantTilesFloats[index])).ToString().Substring(0, 3);
                    string secondSub = ((TileType)((int)relevantTilesFloats[index])).ToString().Substring(((TileType)((int)relevantTilesFloats[index])).ToString().Length - 1, 1);
                    toReturn += firstSub + secondSub + "\t";
                }
                toReturn += "\n\n";
            }
            return toReturn;
        }

        public void UpdateInformation(Tile[,] state)
        {
            // UpdatePossibleMoves(state);
            // UpdateRelevantTiles(possibleMoves);
            GenerateFloats(state);
            // Debug.Log(LogTileFloats(state));
        }

        /* private void UpdatePossibleMoves(Tile[,] state)
        {
            possibleMoves = state.OfType<Tile>()
                .Where(tile => tile.State == TileState.Unrevealed || tile.State == TileState.Flagged)
                .ToList();
        }

        // ! NOTE: This method is likely very expensive.
        private void UpdateRelevantTiles(List<Tile> possibleMoves)
        {
            relevantTiles.Clear();

            foreach (Tile tile in possibleMoves) {
                if (tile.Type != TileType.Empty) {
                    foreach (Tile neighbour in game.GetAdjacentTiles(tile.gridPosition.x, tile.gridPosition.y))
                        if (!relevantTiles.Contains(neighbour))
                            relevantTiles.Add(neighbour);
                }
            }

            GenerateFloats();
        } */
    }
}