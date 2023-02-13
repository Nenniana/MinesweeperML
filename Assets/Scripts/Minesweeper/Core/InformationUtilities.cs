using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MineSweeper {
    public class InformationUtilities
    {
        [ShowInInspector]
        internal List<Tile> possibleMoves;

        [ShowInInspector]
        internal List<Tile> relevantTiles;

        internal List<float> relevantTilesFloats;

        private Game game;

        public void Initialize(Tile[,] state, Game _game) {
            game = _game;
            // possibleMoves = new List<Tile>();
            // relevantTiles = new List<Tile>();
            relevantTilesFloats = new List<float>();

            // possibleMoves = state.OfType<Tile>().ToList();
            // relevantTiles = possibleMoves;
            GenerateFloats(state);
        }

        private void GenerateFloats(Tile[,] state) {
            relevantTilesFloats.Clear();

            int width = state.GetLength(0);
            int height = state.GetLength(1);

            for (int x = 0; x < state.GetLength(0); x++)
            {
                for (int y = 0; y < state.GetLength(1); y++)
                {
                    float enumValue = 0f;

                    /* if (tile.State == TileState.Unrevealed)
                        enumValue = (float)TileType.Unrevealed;
                    else */
                    enumValue = (float)state[x, y].Type;

                    relevantTilesFloats.Add(enumValue);
                }
            }
        }

        public void UpdateInformation(Tile[,] state) {
            // UpdatePossibleMoves(state);
            // UpdateRelevantTiles(possibleMoves);
            GenerateFloats(state);
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