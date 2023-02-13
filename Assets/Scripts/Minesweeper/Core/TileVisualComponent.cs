using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper {
    public class TileVisualComponent : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TMPro.TextMeshProUGUI text, rightText, leftText;

        public void InitializeTileVisual(TileVisual tileVisual, Vector2Int gridPosition)
        {
            UpdateTileVisual(tileVisual);
            transform.localPosition = new Vector3(gridPosition.x, gridPosition.y, 0);
        }

        public void UpdateTileVisual(TileVisual tileVisual)
        {
            image.color = tileVisual.color;
            text.text = tileVisual.symbol;
            text.color = tileVisual.textColor;
            rightText.color = tileVisual.textColor;
            leftText.color = tileVisual.textColor;
        }

        public void UpdateRightText(string text)
        {
            rightText.text = text;
        }

        public void UpdateLeftText(string text)
        {
            leftText.text = text;
        }

        public void NullLeftRightText() {
            leftText.text = "";
            rightText.text = "";
        }

    }
}