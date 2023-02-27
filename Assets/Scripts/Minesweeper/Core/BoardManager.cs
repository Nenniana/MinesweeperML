using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private int height = 8;
    [SerializeField]
    private int width = 8;
    [SerializeField]
    private int mineCount = 1;
    [SerializeField]
    private bool useCurriculumMineCount = false;

    [HideInInspector]
    public static BoardManager Instance { get; private set; }
    public int Height { get => height; private set => height = value; }
    public int Width { get => width; private set => width = value; }
    public int MineCount { get => mineCount; private set => mineCount = value; }
    public bool UseCurriculumMineCount { get => useCurriculumMineCount; private set => useCurriculumMineCount = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
