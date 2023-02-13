using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace MineSweeper
{
    public class Board : SerializedMonoBehaviour
    {
        [SerializeField]
        private GameObject tileVisualObject;

        [SerializeField]
        private Dictionary<TileType, TileVisual> tileVisuals;

        private TileVisualComponent[,] visualState;

        private List<TileVisualComponent> specialTileVisualComponents = new List<TileVisualComponent>();

        private Game game;

        [Button]
        public void FillDictonary() {
            if (tileVisuals == null || tileVisuals.Count == 0)
                tileVisuals = new Dictionary<TileType, TileVisual>();

            foreach (TileType tileType in Enum.GetValues(typeof(TileType)).Cast<TileType>().ToArray()) {
                if (!tileVisuals.ContainsKey(tileType)) {
                    tileVisuals.Add(tileType, ScriptableObject.CreateInstance<TileVisual>());
                }
            }
        }

        private void OnDisable() {
            game.OnNewMove -= OnNewMoveMade;
        }

        public void ResetBoardVisuals() {
            foreach (TileVisualComponent visualComponent in visualState) {
                // Debug.Log("Resetting.");
                visualComponent.UpdateTileVisual(tileVisuals[TileType.Unrevealed]);
            }
        }

        public void GenerateBoardVisuals(Tile[,] state, Game _game) {
            game = _game;
            game.OnNewMove += OnNewMoveMade;

            InitializeBoardVisuals(state);
        }

        public void InitializeBoardVisuals(Tile[,] state) {
            int width = state.GetLength(0);
            int height = state.GetLength(1);

            visualState = new TileVisualComponent[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileVisualComponent tileComponent = Instantiate(tileVisualObject, gameObject.transform).GetComponent<TileVisualComponent>();
                    tileComponent.InitializeTileVisual(GetTileVisual(state[x, y]), state[x, y].gridPosition);
                    visualState[x,y] = tileComponent;
                }
            }
        }

        public void UpdateTileVisual(Tile tile) {
            visualState[tile.gridPosition.x, tile.gridPosition.y].UpdateTileVisual(GetTileVisual(tile));
        }

        public void UpdateTileVisualLeftText(Tile tile, string text)
        {
            visualState[tile.gridPosition.x, tile.gridPosition.y].UpdateLeftText(text);
            specialTileVisualComponents.Add(visualState[tile.gridPosition.x, tile.gridPosition.y]);
        }

        public void UpdateTileVisualRightText(Tile tile, string text)
        {
            visualState[tile.gridPosition.x, tile.gridPosition.y].UpdateRightText(text);
            specialTileVisualComponents.Add(visualState[tile.gridPosition.x, tile.gridPosition.y]);
        }

        private TileVisual GetTileVisual(Tile tile) {
            switch (tile.State) {
                case TileState.Unrevealed: return tileVisuals[TileType.Unrevealed];
                case TileState.Exploded: return tileVisuals[TileType.Exploded];
                case TileState.Flagged: return tileVisuals[TileType.Flagged];
                case TileState.Revealed: return tileVisuals[tile.Type];
                default: return null;
            }
        }

        private void OnNewMoveMade(Move obj)
        {
            if (specialTileVisualComponents != null && specialTileVisualComponents.Count > 0) {
                specialTileVisualComponents.ForEach(c => c.NullLeftRightText());
            }
        }
    }
}