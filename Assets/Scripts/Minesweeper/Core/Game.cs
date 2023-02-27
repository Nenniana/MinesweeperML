using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.MLAgents;

namespace MineSweeper
{
    public class Game : MonoBehaviour
    {
        public Action OnGameStarted;
        public Action<Tile[,]> OnGameLoaded;
        public Action<Move> OnNewMove;
        public Action OnGameWon;
        public Action OnGameLost;
        public Action<float> OnScoreUpdate;
        public Action OnIndicatedMove;
        // public Action<int, int> TileRevealed;
        // public void OnTileRevealed(int x, int y) => TileRevealed?.Invoke(x, y);

        [SerializeField]
        private bool copySettingsFromBoardManager = true;

        [SerializeField]
        private int width = 16;
        [SerializeField]
        private int height = 16;
        [SerializeField]
        private int mineCount = 1;

        private float gridSize = 1;

        [SerializeField]
        private float progressWeight = 2;
        [SerializeField]
        private float emptyFloodScore = 0.5f;
        [SerializeField]
        private float normalTileScore = 4f;
        [SerializeField]
        private float alreadyRevealedScore = -0.5f;

        private Board board;
        private Tile[,] state;

        private GameState gameState = GameState.Calculating;

        // [SerializeReference]
        // public MoveUtilities moveUtilities = new MoveUtilities();

        [SerializeReference]
        public InformationUtilities informationUtilities = new InformationUtilities();

        [ShowInInspector]
        private int tilesRevealed;
        [ShowInInspector]
        private float score;


        public float Score { get { return score; } set { score = value; } }

        private void Awake()
        {
            if (copySettingsFromBoardManager)
                CopySettings();

            TryGetComponent<Board>(out board);
            Camera.main.transform.position = new Vector3(gameObject.transform.localPosition.x + width / 2, gameObject.transform.localPosition.x + height / 2, -1);
        }

        private void CopySettings()
        {
            height = BoardManager.Instance.Height;
            width = BoardManager.Instance.Width;
            if (!BoardManager.Instance.UseCurriculumMineCount)
                mineCount = BoardManager.Instance.MineCount;
            else
                mineCount = (int)Academy.Instance.EnvironmentParameters.GetWithDefault("mine_count", 1.0f);
        }

        private void Start()
        {
            InitializeBoardVisuals();
        }

        private void InitializeGame()
        {
            tilesRevealed = 0;
            Score = 0;
            board.ResetBoardVisuals();
            GameStarted();
        }

        private void InitializeBoardVisuals()
        {
            GenerateBoardMechanics();
            board.GenerateBoardVisuals(state, this);
            GameStarted();
        }

        private void GenerateBoardMechanics()
        {
            state = new Tile[width, height];
            GenerateTiles();
            GenerateMines();
            GenerateNumbers();
            informationUtilities.Initialize(state, this);
        }

        [Button]
        public void NewGame()
        {
            CheckCurriculum();
            GenerateBoardMechanics();
            InitializeGame();
        }

        private void CheckCurriculum()
        {
            if (BoardManager.Instance.UseCurriculumMineCount)
                mineCount = (int)Academy.Instance.EnvironmentParameters.GetWithDefault("mine_count", 1.0f);
        }

        private void GameStarted()
        {
            OnGameStarted?.Invoke();
            gameState = GameState.InProgress;
        }

        [Button]
        private void LoadMoveState(int moveIndex)
        {
            // LoadGame(moveUtilities.GetMove(moveIndex).state);
        }

        [Button]
        private void PreviousMoveState()
        {
            // LoadGame(moveUtilities.GetPreviousMove().state);
        }

        private void LoadGame(Tile[,] newState)
        {
            state = newState.Clone() as Tile[,];
            InitializeGame();
            OnGameLoaded?.Invoke(state);
            // IndicateMove();
        }

        /* private void IndicateMove()
        {
            Vector2Int tilePosition = moveUtilities.GetCurrentMove().gridPosition;
            if (moveUtilities.GetCurrentMove().moveType == MoveType.Default)
                board.UpdateTileVisualLeftText(state[tilePosition.x, tilePosition.y], "ID");
            else
                board.UpdateTileVisualLeftText(state[tilePosition.x, tilePosition.y], "IF");

            board.UpdateTileVisual(state[tilePosition.x, tilePosition.y]);
        } */

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Vector2Int movePosition = GetMousePosition();

                if (Input.GetMouseButtonDown(1))
                {
                    Move(movePosition.x, movePosition.y, 1);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Move(movePosition.x, movePosition.y, 0);
                }
            }
        }

        public void Move(int x, int y, int reveal)
        {
            if (gameState == GameState.InProgress)
            {
                CheckWinCondition();
                OnIndicatedMove?.Invoke();
                // Debug.Log("Move was made. Game is in: " + gameState + " state.");

                if (reveal == 0)
                    Reveal(x, y);
                else
                    Flag(x, y);

                board.UpdateTileVisualRightText(state[x, y], "C");
                informationUtilities.UpdateInformation(state);
            }
        }

        private void Reveal(int x, int y)
        {
            if (IsValidTile(x, y))
            {
                if (state[x, y].State != TileState.Revealed)
                {

                    /* moveUtilities.SaveMove(MoveType.Default, new Vector2Int(x, y), state);
                    OnNewMove?.Invoke(moveUtilities.GetCurrentMove()); */

                    Tile tile = state[x, y];

                    if (tile.State == TileState.Flagged)
                        return;
                    else if (tile.Type == TileType.Empty)
                        Flood(tile);
                    else if (tile.Type == TileType.Mine)
                        Explode(tile);
                    else
                    {

                        OnScoreUpdate?.Invoke(CalculateReward(normalTileScore));
                        // OnTileRevealed(x, y);
                        tile.State = TileState.Revealed;
                        state[x, y] = tile;
                        board.UpdateTileVisual(tile);
                        tilesRevealed++;
                        CheckWinCondition();
                    }
                }
                else
                {
                    OnScoreUpdate?.Invoke(alreadyRevealedScore);
                }
            }
        }

        private void Explode(Tile tile)
        {
            gameState = GameState.GameOver;

            /* 
            tile.State = TileState.Exploded;
            tile.Type = TileType.Exploded;
            state[tile.gridPosition.x, tile.gridPosition.y] = tile;

            board.UpdateTileVisual(tile);
            RevealAll(true); */

            OnGameLost?.Invoke();
            GameManager.Instance.GameWonLost(false);
        }

        private void RevealAll(bool minesOnly)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (minesOnly)
                    {
                        if (state[x, y].Type == TileType.Mine)
                        {
                            state[x, y].State = TileState.Revealed;
                            board.UpdateTileVisual(state[x, y]);
                        }
                    }
                    else
                    {
                        state[x, y].State = TileState.Revealed;
                        board.UpdateTileVisual(state[x, y]);
                    }
                }
            }
        }

        private void Flood(Tile tile)
        {
            if (tile.Type == TileType.Mine || tile.Type == TileType.Invalid || tile.State == TileState.Revealed)
                return;

            OnScoreUpdate?.Invoke(CalculateReward(emptyFloodScore));
            // OnTileRevealed(tile.gridPosition.x, tile.gridPosition.y);
            tile.State = TileState.Revealed;
            state[tile.gridPosition.x, tile.gridPosition.y] = tile;
            board.UpdateTileVisual(tile);
            tilesRevealed++;
            CheckWinCondition();

            if (tile.Type == TileType.Empty)
            {
                if (IsValidTile(tile.gridPosition.x, tile.gridPosition.y - 1))
                    Flood(state[tile.gridPosition.x, tile.gridPosition.y - 1]);
                if (IsValidTile(tile.gridPosition.x, tile.gridPosition.y + 1))
                    Flood(state[tile.gridPosition.x, tile.gridPosition.y + 1]);
                if (IsValidTile(tile.gridPosition.x - 1, tile.gridPosition.y))
                    Flood(state[tile.gridPosition.x - 1, tile.gridPosition.y]);
                if (IsValidTile(tile.gridPosition.x + 1, tile.gridPosition.y))
                    Flood(state[tile.gridPosition.x + 1, tile.gridPosition.y]);
            }

        }

        private void Flag(int x, int y)
        {
            if (IsValidTile(x, y))
            {

                // Cannot flag if already revealed
                if (state[x, y].State == TileState.Revealed || state[x, y].Type == TileType.Invalid)
                    return;

                if (state[x, y].State == TileState.Flagged)
                {
                    state[x, y].State = TileState.Unrevealed;
                    /* moveUtilities.SaveMove(MoveType.Unflag, new Vector2Int(x, y), state);
                    OnNewMove?.Invoke(moveUtilities.GetCurrentMove()); */
                }
                else if (state[x, y].State == TileState.Unrevealed)
                {
                    state[x, y].State = TileState.Flagged;
                    /* moveUtilities.SaveMove(MoveType.Flag, new Vector2Int(x, y), state);
                    OnNewMove?.Invoke(moveUtilities.GetCurrentMove()); */
                }

                board.UpdateTileVisual(state[x, y]);
            }
        }

        private Vector2Int GetMousePosition()
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2Int gridPosition = GetGridPositionByWorld(worldPosition);
            return gridPosition;
        }

        private Vector2Int GetGridPositionByWorld(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x / gridSize), Mathf.RoundToInt(worldPosition.y / gridSize));
        }

        private void GenerateTiles()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = new Tile(new Vector2Int(x, y), TileType.Empty);
                    state[x, y] = tile;
                }
            }
        }

        private void GenerateMines()
        {
            foreach (var mine in RandomUtilities.GetRandomElements2D(state, mineCount))
            {
                state[mine.gridPosition.x, mine.gridPosition.y].Type = TileType.Mine;
            }
        }

        private float CalculateReward(float value)
        {
            // Debug.Log("Tiles revealed value: " + ((float)tilesRevealed / ((float)height * (float)width)));
            return value * ((((float)tilesRevealed / ((float)height * (float)width)) * progressWeight));
        }

        private void GenerateNumbers()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (state[x, y].Type == TileType.Mine)
                        continue;

                    state[x, y].SetNumber(CountMines(x, y));
                }
            }
        }

        private void CheckWinCondition()
        {
            if (((height * width) - tilesRevealed) <= mineCount)
            {
                gameState = GameState.Won;
                OnGameWon?.Invoke();
                GameManager.Instance.GameWonLost(true);
            }
        }

        private int CountMines(int x, int y)
        {
            int count = 0;

            for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
            {
                for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)
                {
                    if (IsValidTile(adjacentX, adjacentY))
                    {
                        if (adjacentX == x && adjacentY == y)
                            continue;

                        if (state[adjacentX, adjacentY].Type == TileType.Mine)
                            count++;
                    }
                }
            }

            return count;
        }

        internal List<Tile> GetAdjacentTiles(int x, int y)
        {
            List<Tile> adjacentTiles = new List<Tile>();

            for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
            {
                for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)
                {
                    if (IsValidTile(adjacentX, adjacentY))
                    {
                        if (adjacentX == x && adjacentY == y)
                            continue;

                        adjacentTiles.Add(state[adjacentX, adjacentY]);
                    }
                }
            }

            return adjacentTiles;
        }

        private bool IsValidTile(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}