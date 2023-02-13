using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper {
    public struct Tile
    {
        public Action<TileState> OnStateChanged;
        public Action<TileType> OnTypeChanged;

        public Tile(Vector2Int _gridPosition, TileType _type) : this()
        {
            State = TileState.Unrevealed;
            gridPosition = _gridPosition;
            Type = _type;
        }

        private TileType type;
        private TileState state;
        internal Vector2Int gridPosition;
        internal int Number;

        internal TileState State { get { return state; } set { state = value; OnStateChanged?.Invoke(state); } }
        internal TileType Type { get { return type; } set { type = value; OnTypeChanged?.Invoke(type); } }

        internal void SetNumber(int _number) {
            Number = _number;

            if (Number > 0)
                Type = GetTypeByNumber(Number);
        }

        private TileType GetTypeByNumber(int number) {
            switch (Number)
            {
                case 1: return TileType.Number1;
                case 2: return TileType.Number2;
                case 3: return TileType.Number3;
                case 4: return TileType.Number4;
                case 5: return TileType.Number5;
                case 6: return TileType.Number6;
                case 7: return TileType.Number7;
                case 8: return TileType.Number8;
                default: return TileType.Invalid;            
            }
        }

    } 
}