using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MineSweeper
{
    [CreateAssetMenu(fileName = "TileVisual", menuName = "Minesweeper 1.0/TileVisual", order = 0)]
    [InlineEditor(Expanded = true)]
    public class TileVisual : ScriptableObject 
    {
        [SerializeField]
        internal Color color = Color.white;

        [SerializeField]
        internal Color textColor = new Color(51,51,51);

        [SerializeField]
        internal string symbol = "*";
    }
}