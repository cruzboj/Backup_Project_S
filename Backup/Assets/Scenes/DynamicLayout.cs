using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DynamicLayout : MonoBehaviour
{
    private int NumberOfrows = 1;
    public int NumberOfcolumns;

    [Header("UI Canvas")]
    public RectTransform panelColumn;
    public GameObject cell;
    public Transform Board;

    private int rowSize;
    private int columnSize;

    // רשימת צבעים לשחקנים
    private Color32[] playerColors = new Color32[] {
        new Color32(255, 90, 76, 255),     // Red
        new Color32(52, 150, 255, 255),     // blue
        new Color32(169, 255, 76, 255),     // green
        new Color32(255, 255, 76, 255),   // yellow
        new Color32(255, 150, 63, 255),   // orange
        new Color32(255, 63, 176, 255)    // purple
    };

    void Initialize()
    {
        rowSize = NumberOfrows;

        columnSize = PlayerInputManager.instance != null ? PlayerInputManager.instance.playerCount : 1;

        Debug.Log($"Generating Board: Rows = {rowSize}, Columns = {columnSize}");
    }

    void ClearBoard()
    {
        for(int i = 0; i < Board.childCount; i++)
        {
            Destroy(Board.GetChild(i).gameObject);
        }
    }

    public void GenerateBoard()
    {
        ClearBoard();
        Initialize();

        RectTransform colParent;

        for (int colIndex = 0; colIndex < columnSize; colIndex++)
        {
            // יצירת עמודה חדשה
            colParent = Instantiate(panelColumn, Board);

            // קביעת צבע ייחודי לכל שחקן
            Color playerColor = playerColors[colIndex % playerColors.Length];

            // שינוי צבע ה-MaskLayer (panelColumn)
            Image columnImage = colParent.GetComponent<Image>();

            for (int rowIndex = 0; rowIndex < rowSize; rowIndex++)
            {

                GameObject newCell = Instantiate(cell, colParent);

                // שינוי צבע התא
                Image cellImage = newCell.GetComponent<Image>();
                if (cellImage != null)
                {
                    cellImage.color = playerColor;
                }
            }
        }
    }
}
